using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    Move,
    Attack,
    Die
}
public class Unit : MonoBehaviour
{
    #region Fields
    [SerializeField] private int id;
    [SerializeField] private string unitName;
    [SerializeField] private Sprite unitPic;
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

    private NavMeshAgent navAgent;
    #endregion

    #region Properties
    public int ID { get => id; set => id = value; }
    public string UnitName { get => unitName; }
    public Sprite UnitPic { get => unitPic; }
    public int CurHP { get => curHP; set => curHP = value; }
    public int MaxHP { get => maxHP; }
    public int MoveSpeed { get => moveSpeed; }
    public int MinWpnDamage { get => minWpnDamage; }
    public int MaxWpnDamage { get => maxWpnDamage; }
    public int Armour { get => armour; }
    public float VisualRange { get => visualRange; }
    public float WeaponRange { get => weaponRange; }
    public UnitState State { get => state; set => state = value; }
    public NavMeshAgent NavAgent { get => navAgent; }
    public Faction Faction { get => faction; }
    public GameObject SelectionVisual { get => selectionVisual; }
    #endregion
    

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
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
        navAgent.isStopped = true;
        navAgent.ResetPath();
    }
    
    public void MoveToPosition(Vector3 dest)
    {
        if (navAgent != null)
        {
            navAgent.SetDestination(dest);
            navAgent.isStopped = false;
        }

        SetState(UnitState.Move); 
    }
    
    private void MoveUpdate()
    {
        float distance = Vector3.Distance(transform.position, navAgent.destination);

        if (distance <= 1f)
            SetState(UnitState.Idle);
    }


}
