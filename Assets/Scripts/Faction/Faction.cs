using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum Nations
{
    Neutral = 0,
    Britain,
    Pirates,
    France,
    Spain,
    Portugal,
    Netherlands
}

public class Faction : MonoBehaviour
{
    [SerializeField] private Nations nations;
    [SerializeField] private Transform unitsParent;
    [SerializeField] private Transform buildingsParent;
    [SerializeField] private Transform ghostBuildingParent;
    [SerializeField] private Transform startPosition; //start position for Faction
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private GameObject[] ghostBuildingPrefabs;
    [SerializeField] private GameObject[] unitPrefabs;
    
    [Header("Resources")]
    [SerializeField] private int food;
    [SerializeField] private int wood;
    [SerializeField] private int gold;
    [SerializeField] private int stone;
    [SerializeField] private List<Unit> aliveUnits = new List<Unit>();
    [SerializeField] private List<Building> aliveBuildings = new List<Building>();
    [SerializeField] private int newResourceRange = 50; //range for worker to find new resource
    
    private int unitLimit = 6; //Initial unit limit
    private int housingUnitNum = 5; //number of units per each housing
    
    public Nations Nations => nations;
    public Transform UnitsParent => unitsParent;
    public Transform BuildingsParent => buildingsParent;
    public Transform GhostBuildingParent => ghostBuildingParent;
    public Transform StartPosition => startPosition;
    public int Food { get => food; set => food = value; }
    public int Wood { get => wood; set => wood = value; }
    public int Gold { get => gold; set => gold = value; }
    public int Stone { get => stone; set => stone = value; }
    public List<Unit> AliveUnits => aliveUnits;
    public List<Building> AliveBuildings => aliveBuildings;
    public GameObject[] BuildingPrefabs => buildingPrefabs;
    public GameObject[] UnitPrefabs => unitPrefabs;
    public GameObject[] GhostBuildingPrefabs => ghostBuildingPrefabs;
    public int UnitLimit => unitLimit;
    public int HousingUnitNum => housingUnitNum;

    private void Start()
    {
        UpdateHousingLimit();
    }

    public bool CheckUnitCost(Unit unit)
    {
        if (food < unit.UnitCost.Food)
            return false;

        if (wood < unit.UnitCost.Wood)
            return false;

        if (gold < unit.UnitCost.Gold)
            return false;

        if (stone < unit.UnitCost.Stone)
            return false;

        return true;
    }
    
    public void DeductUnitCost(Unit unit)
    {
        food -= unit.UnitCost.Food;
        wood -= unit.UnitCost.Wood;
        gold -= unit.UnitCost.Gold;
        stone -= unit.UnitCost.Stone;
    }
    public bool CheckBuildingCost(Building building)
    {
        if (food < building.StructureCost.food)
            return false;

        if (wood < building.StructureCost.wood)
            return false;

        if (gold < building.StructureCost.gold)
            return false;

        if (stone < building.StructureCost.stone)
            return false;

        return true;
    }
    public void DeductBuildingCost(Building building)
    {
        food -= building.StructureCost.food;
        wood -= building.StructureCost.wood;
        gold -= building.StructureCost.gold;
        stone -= building.StructureCost.stone;
    }
    
    public void GainResource(ResourceType resourceType, int amount)
    {
        switch (resourceType)
        {
            case ResourceType.Food:
                food += amount;
                break;
            case ResourceType.Wood:
                wood += amount;
                break;
            case ResourceType.Gold:
                gold += amount;
                break;
            case ResourceType.Stone:
                stone += amount;
                break;
        }

        if (this == GameManager.Instance.MyFaction)
            MainUI.Instance.UpdateAllResource(this);
    }

    // gets the closest resource to the position (random between nearest 3 for some variance)
    public ResourceSource GetClosestResource(Vector3 pos, ResourceType rType)
    {
        List<ResourceSource> closest = new List<ResourceSource>();

        foreach (ResourceSource resource in ResourceManager.Instance.Resources)
        {
            if (!resource)
                continue;

            if (resource.RsrcType != rType) continue;
            
            float dist = Vector3.Distance(pos, resource.transform.position);
            if (dist > newResourceRange) continue;
            closest.Add(resource);
        }
        ResourceSource randomizedSource = null;
        if (closest.Count > 0)
        {
            randomizedSource = closest[Random.Range(0, closest.Count)];
        }
        return randomizedSource;
    }

    public Vector3 GetHQSpawnPos()
    {
        foreach (Building b in aliveBuildings)
        {
            if (!b) continue;
            if (b.IsHQ)
                return b.SpawnPoint.position;
        }
        return startPosition.position;
    }

    
    public bool IsMyUnit(Unit u)
    {
        return aliveUnits.Contains(u);
    }
    
    public bool IsMyBuilding(Building b)
    {
        return aliveBuildings.Contains(b);
    }

    public void UpdateHousingLimit()
    {
        unitLimit = 6; //starting unit Limit

        foreach (Building b in aliveBuildings)
        {
            if (b.IsHousing && b.IsFunctional)
            {
                unitLimit += housingUnitNum;
            }
        }

        if (unitLimit >= 100)
            unitLimit = 100;
        else if (unitLimit < 0)
            unitLimit = 0;

        if (this != GameManager.Instance.MyFaction) return;
        MainUI.Instance.UpdateAllResource(this);
    }
    
    public bool CheckUnitCost(int i)
    {
        Unit unit = unitPrefabs[i].GetComponent<Unit>();

        if (!unit)
            return false;

        if (food < unit.UnitCost.Food)
            return false;

        if (wood < unit.UnitCost.Wood)
            return false;

        if (gold < unit.UnitCost.Gold)
            return false;

        if (stone < unit.UnitCost.Stone)
            return false;

        return true;
    }
    
    public Color GetNationColor()
    {
        Color col;

        switch(nations)
        {
            case Nations.Neutral:
                col = Color.white;
                break;
            case Nations.Britain:
                col = Color.red;
                break;
            case Nations.Pirates:
                col = Color.black;
                break;
            case Nations.France:
                col = Color.blue;
                break;
            case Nations.Spain:
                col = Color.yellow;
                break;
            case Nations.Portugal:
                col = Color.green;
                break;
            case Nations.Netherlands:
                col = new Color32 (255, 157, 0, 255);
                break;
            default:
                col = Color.white;
                break;
        }
        return col;
    }
}
