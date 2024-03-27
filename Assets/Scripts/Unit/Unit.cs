using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    Move,
    AttackUnit,
    MoveToBuild,
    BuildProgress,
    MoveToResource,
    Gather,
    DeliverToHQ,
    StoreAtHQ,
    MoveToEnemy,
    MoveToEnemyBuilding,
    AttackBuilding,
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
    [SerializeField] private Unit curEnemyUnitTarget;
    [SerializeField] private Building curEnemyBuildingTarget;
    [SerializeField] private float attackRate = 1f; //how frequent this unit attacks in second
    [SerializeField] private float lastAttackTime;
    [SerializeField] private float defendRange = 30f; //the range that a unit will defensively auto-attack

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
    public float DefendRange => defendRange;

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
            case UnitState.MoveToEnemy:
                MoveToEnemyUpdate();
                break;
            case UnitState.AttackUnit:
                AttackUpdate();
                break;
            case UnitState.MoveToEnemyBuilding:
                MoveToEnemyBuildingUpdate();
                break;
            case UnitState.AttackBuilding:
                AttackBuildingUpdate();
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
    
    protected virtual IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    
    // called when my health reaches zero
    protected virtual void Die()
    {
        NavAgent.isStopped = true;

        SetState(UnitState.Die);

        if (faction)
            faction.AliveUnits.Remove(this);

        InfoManager.Instance.ClearAllInfo();  
        //Debug.Log(gameObject + " dies.");
        StartCoroutine("DestroyObject");
    }
    
    // move to an enemy unit and attack them
    public void ToAttackUnit(Unit target)
    {
        if (curHP <= 0 || state == UnitState.Die)
            return;
        curEnemyUnitTarget = target;
        SetState(UnitState.MoveToEnemy);
    }

    // called when an enemy unit attacks us
    public void TakeDamage(Unit enemy, int damage)
    {
        //I'm already dead
        if (curHP <= 0 || state == UnitState.Die)
            return;

        curHP -= damage;

        if (curHP <= 0)
        {
            curHP = 0;
            Die();
        }

        if (!IsWorker) //if this unit is not worker
            ToAttackUnit(enemy); //always counter-attack
    }

    
    // called every frame the 'MoveToEnemy' state is active
    public void MoveToEnemyUpdate()
    {
        // if our target is null, go idle
        if (!curEnemyUnitTarget)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            lastPathUpdateTime = Time.time;
            NavAgent.isStopped = false;

            if (curEnemyUnitTarget)
                NavAgent.SetDestination(curEnemyUnitTarget.transform.position);
        }

        if (Vector3.Distance(transform.position, curEnemyUnitTarget.transform.position) <= WeaponRange)
            SetState(UnitState.AttackUnit);
    }
    
    // called every frame the 'Attack' state is active
    protected void AttackUpdate()
    {
        // if our target is dead, go idle
        if (!curEnemyUnitTarget || curEnemyUnitTarget.CurHP <= 0)
        {
            //DisableAllWeapons();
            SetState(UnitState.Idle);
            return;
        }

        // if we're still moving, stop
        if (!NavAgent.isStopped)
            NavAgent.isStopped = true;

        // look at the enemy
        LookAt(curEnemyUnitTarget.transform.position);

        // attack every 'attackRate' seconds
        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            curEnemyUnitTarget.TakeDamage(this, UnityEngine.Random.Range(minWpnDamage, maxWpnDamage + 1));
        }

        // if we're too far away, move towards the enemy
        if (Vector3.Distance(transform.position, curEnemyUnitTarget.transform.position) > weaponRange)
        {
            SetState(UnitState.MoveToEnemy);
            //Debug.Log($"{unitName} - From Attack Update");
        }
    }
    
    // move to an enemy building and attack them
    public void ToAttackBuilding(Building target)
    {
        curEnemyBuildingTarget = target;
        SetState(UnitState.MoveToEnemyBuilding);
    }
    
    // called every frame the 'MoveToEnemyBuilding' state is active
    private void MoveToEnemyBuildingUpdate()
    {
        if (curEnemyBuildingTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            lastPathUpdateTime = Time.time;
            NavAgent.isStopped = false;
            NavAgent.SetDestination(curEnemyBuildingTarget.transform.position);
        }

        if ((Vector3.Distance(transform.position, curEnemyBuildingTarget.transform.position) - 4f) <= WeaponRange)
        {
            SetState(UnitState.AttackBuilding);
        }
    }
    
    // called every frame the 'AttackBuilding' state is active
    private void AttackBuildingUpdate()
    {
        // if our target is dead, go idle
        if (curEnemyBuildingTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        // if we're still moving, stop
        if (!NavAgent.isStopped)
        {
            NavAgent.isStopped = true;
        }

        // look at the enemy
        LookAt(curEnemyBuildingTarget.transform.position);

        // attack every 'attackRate' seconds
        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;

            curEnemyBuildingTarget.TakeDamage(UnityEngine.Random.Range(minWpnDamage, maxWpnDamage + 1));
        }

        // if we're too far away, move towards the enemy's building
        if ((Vector3.Distance(transform.position, curEnemyBuildingTarget.transform.position) - 4f) > WeaponRange)
        {
            SetState(UnitState.MoveToEnemyBuilding);
        }
    }
    public void ToAttackTurret(Turret turret)
    {
        if (curHP <= 0 || state == UnitState.Die)
            return;
        curEnemyBuildingTarget = turret;
        SetState(UnitState.MoveToEnemyBuilding);
    }

    public void TakeDamage(Turret turret, int damage)
    {
        //I'm already dead
        if (curHP <= 0 || state == UnitState.Die)
            return;

        curHP -= damage;

        if (curHP <= 0)
        {
            curHP = 0;
            Die();
        }

        if (!IsWorker) //if this unit is not worker
            ToAttackTurret(turret); //counter-attack at turret
    }

}
