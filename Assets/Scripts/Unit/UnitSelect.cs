using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSelect : MonoBehaviour
{
    public static UnitSelect Instance;
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private List<Unit> curUnits; //current selected single unit
    [SerializeField] private Building curBuilding; //current selected single building
    [SerializeField] private ResourceSource curResource;
    [SerializeField] private Unit curEnemy;
    [SerializeField] private RectTransform selectionBox;
    
    private Vector2 oldAnchoredPos;//Box old anchored position
    private Vector2 startPos;
    private Camera cam;
    private Faction faction;
    private float timer = 0f;
    private float timeLimit = 0.5f;

    public List<Unit> CurUnits => curUnits;
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
        selectionBox = MainUI.Instance.SelectionBox;
    }
    
    void Update()
    {
        //mouse down
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            ClearEverything();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }

        // mouse up
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox(Input.mousePosition);
            TrySelect(Input.mousePosition);
        }
        
        timer += Time.deltaTime;
        if (!(timer > timeLimit)) return;
        timer = 0f;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (curUnits.Count == 1)
            ShowUnit(curUnits[0]);
        else if (curEnemy != null)
            ShowEnemyUnit(curEnemy);
        else if (curResource != null)
            ShowResource();
        else if (curBuilding != null)
        {
            if (GameManager.Instance.MyFaction.IsMyBuilding(curBuilding))
                ShowBuilding(curBuilding);//Show building info
            else
                ShowEnemyBuilding(curBuilding);
        }
    }
    
    private void SelectUnit(RaycastHit hit)
    {
        Unit unit = hit.collider.GetComponent<Unit>();

        Debug.Log("Selected Unit");

        if (GameManager.Instance.MyFaction.IsMyUnit(unit))
        {
            curUnits.Add(unit);
            unit.ToggleSelectionVisual(true);
            ShowUnit(unit);
        }
        else
        {
            curEnemy = unit;
            curEnemy.ToggleSelectionVisual(true);
            ShowEnemyUnit(unit);
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
        foreach (Unit unit in curUnits)
            unit.ToggleSelectionVisual(false);
        if (curBuilding)
            curBuilding.ToggleSelectionVisual(false);
        if (curResource)
            curResource.ToggleSelectionVisual(false);
        if (curEnemy)
            curEnemy.ToggleSelectionVisual(false);
    }
    
    private void ClearEverything()
    {
        ClearAllSelectionVisual();
        curUnits.Clear();
        curBuilding = null;
        curResource = null;
        curEnemy = null;
        
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

        if (GameManager.Instance.MyFaction.IsMyBuilding(curBuilding))
        {
            Debug.Log("my building");
            ShowBuilding(curBuilding); //Show building info
        }
        else
        {
            ShowEnemyBuilding(curBuilding);
        }
    }
    
    private void ResourceSelect(RaycastHit hit)
    {
        curResource = hit.collider.GetComponent<ResourceSource>();
        if (!curResource)
            return;

        curResource.ToggleSelectionVisual(true);
        ShowResource();//Show resource info
    }

    private void UpdateSelectionBox(Vector3 mousePos)
    {
        //Debug.Log("Mouse Pos - " + curMousePos);
        if (!selectionBox.gameObject.activeInHierarchy && !curBuilding)
            selectionBox.gameObject.SetActive(true);

        float width = mousePos.x - startPos.x;
        float height = mousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);

        //store old position for real unit selection
        oldAnchoredPos = selectionBox.anchoredPosition;
    }

    private void ReleaseSelectionBox(Vector2 mousePos)
    {
        //Debug.Log("Step 2 - " + _doubleClickMode);
        Vector2 min; //down-left corner
        Vector2 max; //top-right corner

        selectionBox.gameObject.SetActive(false);

        min = oldAnchoredPos - (selectionBox.sizeDelta / 2);
        max = oldAnchoredPos + (selectionBox.sizeDelta / 2);

        //Debug.Log("min = " + min);
        //Debug.Log("max = " + max);

        foreach (Unit unit in GameManager.Instance.MyFaction.AliveUnits)
        {
            Vector2 unitPos = cam.WorldToScreenPoint(unit.transform.position);

            if (unitPos.x > min.x && unitPos.x < max.x && unitPos.y > min.y && unitPos.y < max.y)
            {
                curUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
        selectionBox.sizeDelta = new Vector2(0, 0); //clear Selection Box's size;
    }

    private void ShowEnemyUnit(Unit u)
    {
        InfoManager.Instance.ShowEnemyAllInfo(u);
    }
    
    private void ShowEnemyBuilding(Building b)
    {
        InfoManager.Instance.ShowEnemyAllInfo(b);
    }

    

}
