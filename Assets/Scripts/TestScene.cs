using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    [SerializeField]
    private Unit[] units;
    
    public void SetIdle()
    {
        foreach (Unit unit in units)
        {
            unit.State = UnitState.Idle;
        }
    }

    public void SetMove()
    {
        foreach (Unit unit in units)
        {
            unit.State = UnitState.Move;
        }
    }

    public void SetAttack()
    {
        foreach (Unit unit in units)
        {
            unit.State = UnitState.AttackUnit;
        }
    }
}
