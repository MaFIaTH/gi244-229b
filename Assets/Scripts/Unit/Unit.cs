using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    Move,
    Attack,
    MoveToBuild,
    BuildProgress,
    MoveToResource,
    Gather,
    DeliverToHQ,
    StoreAtHQ,
    Die
}

[Serializable]
public struct UnitCost
{
    [SerializeField] private int food;
    [SerializeField] private int wood;
    [SerializeField] private int gold;
    [SerializeField] private int stone;

    public int Food => food;
    public int Wood => wood;
    public int Gold => gold;
    public int Stone => stone;
}

public class Unit : MonoBehaviour
{
    #region Fields
    [SerializeField] private int id;
    [SerializeField] private string unitName;
    [SerializeField] private Sprite unitPic;
    [SerializeField] private bool isBuilder;
    [SerializeField] private Builder builder;
    [SerializeField] private bool isWorker;
    [SerializeField] private Worker worker;
    [SerializeField] private int curHP;
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int minWpnDamage;
    [SerializeField] private int maxWpnDamage;
    [SerializeField] private int armour;
    [SerializeField] private float visualRange;
    [SerializeField] private float weaponRange;
    [SerializeField] private UnitState state;
    [SerializeField] private Faction faction;
    [SerializeField] private GameObject selectionVisual;
    [SerializeField] private UnitCost unitCost;
    [SerializeField] private float unitWaitTime = 0.1f;
    [SerializeField] private float pathUpdateRate = 1.0f;
    [SerializeField] private float lastPathUpdateTime;
    #endregion

    #region Properties

    public float PathUpdateRate => pathUpdateRate;

    public float LastPathUpdateTime
    {
        get => lastPathUpdateTime;
        set => lastPathUpdateTime = value;
    }
    public bool IsBuilder
    {
        get => isBuilder;
        set => isBuilder = value;
    }

    public Builder Builder => builder;

    public int ID
    {
        get => id;
        set => id = value;
    }

    public string UnitName => unitName;
    public Sprite UnitPic => unitPic;

    public int CurHP
    {
        get => curHP;
        set => curHP = value;
    }

    public int MaxHP => maxHP;
    public int MoveSpeed => moveSpeed;
    public int MinWpnDamage => minWpnDamage;
    public int MaxWpnDamage => maxWpnDamage;
    public int Armour => armour;
    public float VisualRange => visualRange;
    public float WeaponRange => weaponRange;

    public UnitState State
    {
        get => state;
        set => state = value;
    }

    public Faction Faction
    {
        get => faction;
        set => faction = value;
    }

    public NavMeshAgent NavAgent { get; private set; }
    public GameObject SelectionVisual => selectionVisual;
    public UnitCost UnitCost => unitCost;
    public float UnitWaitTime => unitWaitTime;

    public bool IsWorker
    {
        get => isWorker;
        set => isWorker = value;
    }
    
    public Worker Worker => worker;
    #endregion
    

    private void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        if (isBuilder) builder = GetComponent<Builder>();
        if (isWorker) worker = GetComponent<Worker>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        switch (state)
        {
            case UnitState.Move:
                MoveUpdate();
                break;
        }
    }
    
    public void ToggleSelectionVisual(bool flag)
    {
        if (selectionVisual)
            selectionVisual.SetActive(flag);
    }
    
    public void SetState(UnitState toState)
    {
        state = toState;

        if (state != UnitState.Idle) return;
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
    }
    
    public void MoveToPosition(Vector3 dest)
    {
        if (NavAgent)
        {
            NavAgent.SetDestination(dest);
            NavAgent.isStopped = false;
        }

        SetState(UnitState.Move); 
    }
    
    private void MoveUpdate()
    {
        float distance = Vector3.Distance(transform.position, NavAgent.destination);

        if (distance <= 1f)
            SetState(UnitState.Idle);
    }
    
    public void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }


}
