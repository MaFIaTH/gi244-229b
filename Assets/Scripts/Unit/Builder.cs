using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{
    [SerializeField] private bool toBuild = false; //this builder has duty to build things
    [SerializeField] private bool showGhost = false; //ghost building is showing
    [SerializeField] private GameObject[] buildingList; // Buildings that this unit can build
    [SerializeField] private GameObject[] ghostBuildingList; // Transparent buildings according to building list
    [SerializeField] private GameObject newBuilding; // Current building to build
    [SerializeField] private GameObject ghostBuilding; // Transparent building to check site to build
    [SerializeField] private Building inProgressBuilding; // The building a unit is currently building

    private Unit unit;
    public GameObject[] BuildingList => buildingList;

    public GameObject NewBuilding
    {
        get => newBuilding;
        set => newBuilding = value;
    }

    public GameObject GhostBuilding
    {
        get => ghostBuilding;
        set => ghostBuilding = value;
    }

    public Building InProgressBuilding
    {
        get => inProgressBuilding;
        set => inProgressBuilding = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void ToCreateNewBuilding(int i) //Start call from ActionManager UI Btns
    {
        if (buildingList[i] == null)
            return;

        Building b = buildingList[i].GetComponent<Building>();

        if (!unit.Faction.CheckBuildingCost(b)) //don't have enough resource to build
            return;
        //Create ghost building at the mouse position
        ghostBuilding = Instantiate(ghostBuildingList[i],
            Input.mousePosition,
            Quaternion.identity, unit.Faction.GhostBuildingParent);

        toBuild = true;
        newBuilding = buildingList[i]; //Set prefab into new building
        showGhost = true;
    }
    
    private void GhostBuildingFollowsMouse()
    {
        if (!showGhost) return;
        Ray ray = CameraController.Instance.Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Ground"))) return;
        if (!ghostBuilding) return;
        ghostBuilding.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
    }
    
    private void CancelToBuild()
    {
        toBuild = false;
        showGhost = false;

        newBuilding = null;
        Destroy(ghostBuilding);
        ghostBuilding = null;
        //Debug.Log("Cancel Building");
    }
    
    public void BuilderStartFixBuilding(Building target)
    {
        inProgressBuilding = target;        
        unit.SetState(UnitState.MoveToBuild);
    }
    
    private void StartConstruction(Building buildingObj)
    {
        BuilderStartFixBuilding(buildingObj);
    }

    public void CreateBuildingSite(Vector3 pos) //Set a building site
    {
        if (ghostBuilding)
        {
            Destroy(ghostBuilding);
            ghostBuilding = null;
        }

        //We use prefab position.y when instantiating.
        GameObject buildingObj = Instantiate(newBuilding,
            new Vector3(pos.x, newBuilding.transform.position.y, pos.z),
            Quaternion.identity);

        newBuilding = null; //Clear 

        Building building = buildingObj.GetComponent<Building>();

        //Set building to be underground
        var buildPosition = buildingObj.transform.position;
        buildPosition = new Vector3(buildPosition.x,
            buildPosition.y - building.IntoTheGround,
            buildPosition.z);
        buildingObj.transform.position = buildPosition;

        //Set building's parent game object
        buildingObj.transform.parent = unit.Faction.BuildingsParent.transform;

        inProgressBuilding = building; //set a new clone building object to be a building in Unit's mind
        unit.Faction.AliveBuildings.Add(building);

        building.Faction = unit.Faction; //set a building's faction to be belong to this player
        building.IsFunctional = false;
        building.CurHP = 1;

        unit.Faction.DeductBuildingCost(building);

        toBuild = false; //Disable flag at the builder
        showGhost = false; //Disable to show ghost building

        if (unit.Faction == GameManager.Instance.MyFaction)
        {
            MainUI.Instance.UpdateAllResource(unit.Faction);
        }
        //Debug.Log("Building site created.");

        //order builders to build together
        StartConstruction(inProgressBuilding);
    }
    
    private void CheckClickOnGround()
    {
        Ray ray = CameraController.Instance.Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;
        bool canBuild = ghostBuilding.GetComponent<FindBuildingSite>().CanBuild;
        //Debug.Log(hit.collider.tag);
        if ((hit.collider.CompareTag("Ground")) && canBuild)
        {
            //Debug.Log("Click Ground to Build");
            CreateBuildingSite(hit.point); //Create building site with 1 HP
        }
    }
    
    void Update()
    {
        if (unit.State == UnitState.Die)
            return;

        if (toBuild) // if this unit is to build something
        {
            GhostBuildingFollowsMouse();
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                CheckClickOnGround();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                CancelToBuild();
        }

        switch (unit.State)
        {
            case UnitState.MoveToBuild:
                MoveToBuild(inProgressBuilding);
                break;
            case UnitState.BuildProgress:
                BuildProgress();
                break;
        }
    }
    
    private void MoveToBuild(Building b)
    {
        if (!b)
            return;
        
        unit.NavAgent.SetDestination(b.transform.position + new Vector3(0, 0, -b.NavMeshObstacle.size.z / 2));
        unit.NavAgent.isStopped = false;
    }

    private void BuildProgress()
    {
        if (!inProgressBuilding)
            return;

        unit.LookAt(inProgressBuilding.transform.position);
        Building b = inProgressBuilding;

        //building is already finished
        if ((b.CurHP >= b.MaxHP) && b.IsFunctional)
        {
            inProgressBuilding = null; //Clear this job off his mind
            unit.SetState(UnitState.Idle);
            return;
        }
        //constructing
        b.Timer += Time.deltaTime;

        if (!(b.Timer >= b.WaitTime)) return;
        b.Timer = 0;
        b.CurHP++;

        if (b.IsFunctional == false) //if this building is being built, not being fixed
            //Raise up building from the ground
            inProgressBuilding.transform.position += new Vector3(0f, b.IntoTheGround / (b.MaxHP - 1), 0f);

        if (b.CurHP < b.MaxHP) return; //finish
        b.CurHP = b.MaxHP;
        b.IsFunctional = true;

        inProgressBuilding = null; //Clear this job off his mind
        unit.SetState(UnitState.Idle);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (unit.State == UnitState.Die)
            return;

        if (unit == null) return;
        if (!inProgressBuilding) return;
        if (other.gameObject != inProgressBuilding.gameObject) return;
        unit.NavAgent.isStopped = true;
        unit.SetState(UnitState.BuildProgress);
    }
    
    private void OnDestroy()
    {
        if (ghostBuilding != null)
            Destroy(ghostBuilding);
    }



}
