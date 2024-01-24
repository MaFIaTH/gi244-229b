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

        switch (u.State)
        {
            case UnitState.Idle:
                anim.SetBool(IsIdle, true);
                break;
            case UnitState.Move:
                anim.SetBool(IsMove, true);
                break;
            case UnitState.Attack:
                anim.SetBool(IsAttack, true);
                break;
        }
    }
}
