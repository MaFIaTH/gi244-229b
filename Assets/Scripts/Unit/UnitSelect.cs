using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    public static UnitSelect Instance;
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Unit curUnit; //current selected single unit
    
    private Camera cam;
    private Faction faction;

    public Unit CurUnit => curUnit;
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
        }
    }
    
    private void ClearAllSelectionVisual()
    {
        if (curUnit)
            curUnit.ToggleSelectionVisual(false);
    }
    
    private void ClearEverything()
    {
        ClearAllSelectionVisual();
        curUnit = null;
    }


}
