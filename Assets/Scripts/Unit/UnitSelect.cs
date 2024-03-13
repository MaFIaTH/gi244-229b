using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSelect : MonoBehaviour
{
    public static UnitSelect Instance;
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Unit curUnit; //current selected single unit
    [SerializeField] private Building curBuilding; //current selected single building
    [SerializeField] private ResourceSource curResource;
    

    private Camera cam;
    private Faction faction;

    public Unit CurUnit => curUnit;
    public Building CurBuilding => curBuilding;
    void Awake()
    {
        Instance = this;
        faction = GetComponent<Faction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        layerMask = LayerMask.GetMask("Unit", "Building", "Resource", "Ground");
    }
    
    void Update()
    {
        //mouse down
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            ClearEverything();
        }

        // mouse up
        if (Input.GetMouseButtonUp(0))
        {
            TrySelect(Input.mousePosition);
        }
    }
    
    private void SelectUnit(RaycastHit hit)
    {
        curUnit = hit.collider.GetComponent<Unit>();

        curUnit.ToggleSelectionVisual(true);

        Debug.Log("Selected Unit");

        if (GameManager.Instance.MyFaction.IsMyUnit(curUnit))
        {
            ShowUnit(curUnit);
        }
    }
    
    private void TrySelect(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        //if we left-click something
        if (!Physics.Raycast(ray, out hit, 1000, layerMask)) return;
        switch (hit.collider.tag)
        {
            case "Unit":
                SelectUnit(hit);
                break;
            case "Building":
                BuildingSelect(hit);
                break;
            case "Resource":
                ResourceSelect(hit);
                break;
        }
    }
    
    private void ClearAllSelectionVisual()
    {
        if (curUnit)
            curUnit.ToggleSelectionVisual(false);
        if (curBuilding)
            curBuilding.ToggleSelectionVisual(false);
        if (curResource)
            curResource.ToggleSelectionVisual(false);
    }
    
    private void ClearEverything()
    {
        ClearAllSelectionVisual();
        curUnit = null;
        curBuilding = null;
        
        InfoManager.Instance.ClearAllInfo();
        ActionManager.Instance.ClearAllInfo();
    }
    
    private void ShowUnit(Unit u)
    {
        InfoManager.Instance.ShowAllInfo(u);
        if (!u.IsBuilder) return;
        ActionManager.Instance.ShowBuilderMode(u);
    }
    
    private void ShowBuilding(Building b)
    {
        InfoManager.Instance.ShowAllInfo(b);
        ActionManager.Instance.ShowCreateUnitMode(b);
    }
    
    private void ShowResource()
    {
        InfoManager.Instance.ShowAllInfo(curResource);//Show resource info in Info Panel

    }

    
    private void BuildingSelect(RaycastHit hit)
    {
        curBuilding = hit.collider.GetComponent<Building>();
        curBuilding.ToggleSelectionVisual(true);

        if (!GameManager.Instance.MyFaction.IsMyBuilding(curBuilding)) return;
        Debug.Log("my building");
        ShowBuilding(curBuilding);//Show building info
    }
    
    private void ResourceSelect(RaycastHit hit)
    {
        curResource = hit.collider.GetComponent<ResourceSource>();
        if (curResource == null)
            return;

        curResource.ToggleSelectionVisual(true);
        ShowResource();//Show resource info
    }




    

}
