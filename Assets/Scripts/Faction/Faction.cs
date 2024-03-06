using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    [Header("Resources")]
    [SerializeField] private int food;
    [SerializeField] private int wood;
    [SerializeField] private int gold;
    [SerializeField] private int stone;
    [SerializeField] private List<Unit> aliveUnits = new List<Unit>();
    [SerializeField] private List<Building> aliveBuildings = new List<Building>();


    public Transform UnitsParent => unitsParent;
    public Transform BuildingsParent => buildingsParent;
    public Transform GhostBuildingParent => ghostBuildingParent;
    public int Food { get => food; set => food = value; }
    public int Wood { get => wood; set => wood = value; }
    public int Gold { get => gold; set => gold = value; }
    public int Stone { get => stone; set => stone = value; }
    public List<Unit> AliveUnits => aliveUnits;
    public List<Building> AliveBuildings => aliveBuildings;

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

    
    public bool IsMyUnit(Unit u)
    {
        return aliveUnits.Contains(u);
    }
    
    public bool IsMyBuilding(Building b)
    {
        return aliveBuildings.Contains(b);
    }



}
