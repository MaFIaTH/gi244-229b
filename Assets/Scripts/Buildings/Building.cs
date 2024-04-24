using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building : Structure
{
    [SerializeField] private bool isFunctional = true;
    [SerializeField] private bool isHQ;
    [SerializeField] private float intoTheGround = 5f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform rallyPoint;
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private List<Unit> recruitList = new List<Unit>();
    [SerializeField] private float unitTimer = 0f;
    [SerializeField] private int curUnitProgress = 0;
    [SerializeField] private float curUnitWaitTime = 0f;
    [SerializeField] private float baseUnitSpawnTime = 10f;
    [SerializeField] private float unitSpawnTime = 10f;
    [SerializeField] private bool isHousing;
    [SerializeField] private bool isBarrack;
    
    private NavMeshObstacle navMeshObstacle;
    private float timer = 0f; //Constructing timer
    private float waitTime = 0.5f; //How fast it will be construct, higher is longer
    
    public NavMeshObstacle NavMeshObstacle => navMeshObstacle;
    public float WaitTime 
    { 
        get => waitTime;
        set => waitTime = value;
    }
    public float Timer 
    { 
        get => timer;
        set => timer = value;
    }
    public float BaseUnitSpawnTime 
    { 
        get => baseUnitSpawnTime;
        set => baseUnitSpawnTime = value;
    }
    public float UnitSpawnTime 
    { 
        get => unitSpawnTime;
        set => unitSpawnTime = value;
    }
    public GameObject[] UnitPrefabs => unitPrefabs;
    public Transform SpawnPoint => spawnPoint;
    public Transform RallyPoint => rallyPoint;
    public bool IsFunctional { get => isFunctional; set => isFunctional = value; }
    public bool IsHQ => isHQ;
    public float IntoTheGround => intoTheGround;
    public bool IsHousing => isHousing;
    public bool IsBarrack => isBarrack;

    private void Awake()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }
    

    // Start is called before the first frame update
    void Start()
    {

    }

// Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.G))
        //     ToCreateUnit(0);
        // else if (Input.GetKeyDown(KeyCode.H))
        //     ToCreateUnit(1);

        if (recruitList.Count <= 0 || !recruitList[0]) return;
        unitTimer += Time.deltaTime;
        curUnitWaitTime = recruitList[0].UnitWaitTime;

        if (unitTimer < curUnitWaitTime) return;
        curUnitProgress++;
        unitTimer = 0f;

        if (curUnitProgress < unitSpawnTime || faction.AliveUnits.Count >= faction.UnitLimit) return;
        curUnitProgress = 0;
        curUnitWaitTime = 0f;
        CreateUnitCompleted();

    }

    
    public void ToCreateUnit(int i)
    {
        Debug.Log(structureName + " creates " + i + ":" + unitPrefabs.Length);
        if (unitPrefabs.Length == 0)
            return;

        if (!unitPrefabs[i])
            return;

        Unit unit = unitPrefabs[i].GetComponent<Unit>();

        if (!unit) 
            return;

        if (!faction.CheckUnitCost(unit)) //not enough resources
            return;

        //Deduct Resource
        faction.DeductUnitCost(unit);

        //If it's me, update UI
        if (faction == GameManager.Instance.MyFaction)
            MainUI.Instance.UpdateAllResource(faction);

        //Add unit into faction's recruit list
        recruitList.Add(unit);

        Debug.Log("Adding" + i + "to Recruit List");
    }
    
    public void CreateUnitCompleted()
    {
        int id = recruitList[0].ID;

        if (!faction.UnitPrefabs[id])
            return;

        GameObject unitObj = Instantiate(faction.UnitPrefabs[id], spawnPoint.position, Quaternion.Euler(0f, 180f, 0f),
            faction.UnitsParent);

        recruitList.RemoveAt(0);

        Unit unit = unitObj.GetComponent<Unit>();
        unit.Faction = faction;
        unit.MoveToPosition(rallyPoint.position); //Go to Rally Point

        //Add unit into faction's Army
        faction.AliveUnits.Add(unit);

        Debug.Log("Unit Recruited");
        //If it's me, update UI
        if (faction == GameManager.Instance.MyFaction)
            MainUI.Instance.UpdateAllResource(faction);
    }
    
    public void ToggleSelectionVisual(bool flag)
    {
        if (SelectionVisual)
            SelectionVisual.SetActive(flag);
    }

    public int CheckNumInRecruitList(int id)
    {
        int num = 0;

        foreach (Unit u in recruitList)
        {
            if (id == u.ID)
                num++;
        }
        return num;
    }   
    
    protected override void Die()
    {
        if (faction)
            faction.AliveBuildings.Remove(this);

        if (IsHousing)
            faction.UpdateHousingLimit();

        if (IsHQ)
        {
            GameManager.Instance.GameOver(true, faction.Nations);
        }

        base.Die();

        //Check Victory Condition
    }


}
