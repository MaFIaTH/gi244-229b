using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionAI : MonoBehaviour
{
    [SerializeField] private Building curHQ;
    [SerializeField] private Building curBarrack;
    [SerializeField] private Building curHunterLodge;
    [SerializeField] private GameObject unfinishedBuilding = null;
    [SerializeField] private Unit specificBuilder;//a builder for fixing any unfinished/broken building
    
    public float checkRate = 1.0f;
    private Faction faction;
    private AISupport support = null;

    void Awake()
    {
        faction = GetComponent<Faction>();
    }

    void Start()
    {
        support = gameObject.GetComponent<AISupport>();
        InvokeRepeating(nameof(Check), 0.0f, checkRate);
    }
    
    private void Check()
    {
        if (faction.AliveBuildings.Count == 0) // if all buildings are destroyed, return
            return;

        //Create Workers
        if (curHQ)
        {
            if (support.Workers.Count < 5) // if there are less than 5 units, keep recruiting Workers
            {
                // if we can recruit a new worker/builder, do so
                if (faction.CheckUnitCost(0))
                    curHQ.ToCreateUnit(0); //HQ recruits a primary worker/builder
            }
        }

        //Create main fighters
        if (!curBarrack) return;
        if ((support.Fighters.Count >= 5)) return;
        if (faction.CheckUnitCost(1))
            curBarrack.ToCreateUnit(0); // recruits main fighter
    }
    
    

}
