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
    
    [Header("Resources")]
    [SerializeField] private int food;
    [SerializeField] private int wood;
    [SerializeField] private int gold;
    [SerializeField] private int stone;
    [SerializeField] private List<Unit> aliveUnits = new List<Unit>();
    [SerializeField] private List<Building> aliveBuildings = new List<Building>();
    
    
    
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
    
    public bool IsMyUnit(Unit u)
    {
        return aliveUnits.Contains(u);
    }
    
    public bool IsMyBuilding(Building b)
    {
        return aliveBuildings.Contains(b);
    }



}
