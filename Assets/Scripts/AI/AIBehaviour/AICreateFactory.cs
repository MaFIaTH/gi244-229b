using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreateFactory : AICreateHQ
{
    
    // Start is called before the first frame update
    void Start()
    {
        support = gameObject.GetComponent<AISupport>();
        buildingPrefab = support.Faction.BuildingPrefabs[4];
        buildingGhostPrefab = support.Faction.GhostBuildingPrefabs[4];
    }
    
    private bool CheckIfAnyUnfinishedFactoryOrBarrack()
    {
        foreach (GameObject factoryObj in support.Factories)
        {
            Building f = factoryObj.GetComponent<Building>();

            if (!f.IsFunctional && (f.CurHP < f.MaxHP)) //This factory is not yet finished
                return true;
        }

        foreach (GameObject barrackObj in support.Barracks)
        {
            Building b = barrackObj.GetComponent<Building>();

            if (!b.IsFunctional && (b.CurHP < b.MaxHP)) //This barrack is not yet finished
                return true;
        }
        return false;
    }
    public override float GetWeight()
    {
        Building b = buildingPrefab.GetComponent<Building>();

        if (!support.Faction.CheckBuildingCost(b)) //Don't have enough resource to build a house
            return 0;
        
        if (CheckIfAnyUnfinishedFactoryOrBarrack()) //Check if there is any unfinished factory
            return 0;

        if (support.Houses.Count > 0 && support.Barracks.Count > 1 && support.Factories.Count == 0) //There are some houses and barracks and there is no factory
            return 2f;

        return 0;
    }
}
