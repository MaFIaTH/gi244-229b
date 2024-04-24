using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Faction myFaction;
    [SerializeField] private Faction enemyFaction;
    //All factions in this game (2 factions for now)
    [SerializeField] private Faction[] factions;

    public static GameManager Instance;

    private float gameTimer;

    public Faction MyFaction => myFaction;
    public Faction EnemyFaction => enemyFaction;
    public Faction[] Factions => factions;

    private void Awake()
    {
        Instance = this;
        SetupPlayers(Settings.mySide, Settings.EnemySide);
        SetupDuration();
    }

    // Start is called before the first frame update
    void Start()
    {
        MainUI.Instance.UpdateAllResource(myFaction);
        CameraController.Instance.FocusOnPosition(myFaction.StartPosition.position);
        gameTimer = (float)Settings.matchDuration;
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            gameTimer = 0;
            GameOver(false);
        }
        MainUI.Instance.UpdateGameTimer(gameTimer);
    }
    
    public void SetupPlayers(Nations myNation, Nations enemyNation)
    {
        foreach (Faction f in factions)
        {
            Debug.Log("Now is :" + f);

            if (f.Nations == myNation)
            {
                Debug.Log("My Side is :" + f);
                myFaction = f;

                f.gameObject.AddComponent<UnitSelect>();
                f.gameObject.AddComponent<UnitCommand>();
            }
            else if (f.Nations == enemyNation)//Enemy
            {
                Debug.Log("Enemy Side is :" + f);
                enemyFaction = f;

                f.gameObject.AddComponent<FactionAI>(); //Routine AI

                f.gameObject.AddComponent<AIController>(); //controller to choose among AI specific commands
                f.gameObject.AddComponent<AISupport>();
                f.gameObject.AddComponent<AIDoNothing>();
                f.gameObject.AddComponent<AIStrike>();
                f.gameObject.AddComponent<AICreateHQ>();
                f.gameObject.AddComponent<AICreateHouse>();
                f.gameObject.AddComponent<AICreateBarrack>();
                f.gameObject.AddComponent<AICreateFactory>();
            }
        }
        
    }
    public void SetupDuration()
    {
        float durationModifier = 1f;
        switch (Settings.matchDuration)
        {
            case MatchDuration.Short:
                durationModifier = 0.5f;
                break;
            case MatchDuration.Medium:
                durationModifier = 1f;
                break;
            case MatchDuration.Long:
                durationModifier = 2f;
                break;
        }
        foreach (Faction f in factions)
        {
            foreach (GameObject b in f.BuildingPrefabs)
            {
                Building building = b.GetComponent<Building>();
                building.MaxHP = (int)(building.BaseMaxHP * durationModifier);
                building.CurHP = building.MaxHP;
                building.UnitSpawnTime = building.BaseUnitSpawnTime * durationModifier;
            }

            foreach (Building b in f.AliveBuildings)
            {
                b.MaxHP = (int)(b.BaseMaxHP * durationModifier);
                b.CurHP = b.MaxHP;
                b.UnitSpawnTime = b.BaseUnitSpawnTime * durationModifier;
            }
        }
    }
    
    public void GameOver(bool manual, Nations loser = default)
    {
        int myAssets = myFaction.AliveBuildings.Count + myFaction.AliveUnits.Count;
        Settings.myAssets = myAssets;
        int enemyAssets = enemyFaction.AliveBuildings.Count + enemyFaction.AliveUnits.Count;
        Settings.enemyAssets = enemyAssets;
        if (manual)
        {
            Settings.gameResult = loser == myFaction.Nations ? GameResult.Lose : GameResult.Win;
        }
        else
        {
            if (myAssets > enemyAssets)
            {
                Settings.gameResult = GameResult.Win;
            }
            else if (myAssets < enemyAssets)
            {
                Settings.gameResult = GameResult.Lose;
            }
            else if (myAssets == enemyAssets)
            {
                Settings.gameResult = GameResult.Draw;
            }
        }
        SceneManager.LoadScene("EndGame");
    }
}
