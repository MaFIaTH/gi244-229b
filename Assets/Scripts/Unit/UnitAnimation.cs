using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private Animator anim;
    private Unit unit;
    private static readonly int IsIdle = Animator.StringToHash("IsIdle");
    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    private static readonly int IsBuilding = Animator.StringToHash("IsBuilding");
    private static readonly int IsChopping = Animator.StringToHash("IsChopping");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        unit = GetComponent<Unit>();
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChooseAnimation(unit);
    }
    
    private void ChooseAnimation(Unit u)
    {
        anim.SetBool(IsIdle, false);
        anim.SetBool(IsMove, false);
        anim.SetBool(IsAttack, false);
        anim.SetBool(IsBuilding, false);
        anim.SetBool(IsChopping, false);
        anim.SetBool(IsDead, false);

        switch (u.State)
        {
            case UnitState.Idle:
            case UnitState.StoreAtHQ:
                anim.SetBool(IsIdle, true);
                break;
            case UnitState.MoveToBuild:
            case UnitState.MoveToResource:
            case UnitState.MoveToEnemy:
            case UnitState.MoveToEnemyBuilding:
            case UnitState.DeliverToHQ:
            case UnitState.Move:
                anim.SetBool(IsMove, true);
                break;
            case UnitState.AttackUnit:
            case UnitState.AttackBuilding:
                anim.SetBool(IsAttack, true);
                break;
            case UnitState.BuildProgress:
                anim.SetBool(IsBuilding, true);
                break;
            case UnitState.Gather:
                anim.SetBool(IsChopping, true);
                break;
            case UnitState.Die:
                anim.SetBool(IsDead, true);
                break;
        }
    }
}
