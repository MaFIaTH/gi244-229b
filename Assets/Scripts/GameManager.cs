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

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MainUI.Instance.UpdateAllResource(myFaction);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
