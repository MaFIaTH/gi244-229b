using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Faction myFaction;
    [SerializeField] private Faction enemyFaction;
    //All factions in this game (2 factions for now)
    [SerializeField] private Faction[] factions;

    public static GameManager Instance;

    public Faction MyFaction => myFaction;
    public Faction EnemyFaction => enemyFaction;
    public Faction[] Factions => factions;

    private void Awake()
    {
        Instance = this;
        SetupPlayers(Settings.mySide, Settings.EnemySide);
    }

    // Start is called before the first frame update
    void Start()
    {
        MainUI.Instance.UpdateAllResource(myFaction);
        CameraController.Instance.FocusOnPosition(myFaction.StartPosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
        }
    }
}
