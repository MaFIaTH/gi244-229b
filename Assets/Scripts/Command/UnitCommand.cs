using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommand : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    
    private UnitSelect unitSelect;
    private Camera cam;
    void Awake()
    {
        unitSelect = GetComponent<UnitSelect>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        layerMask = LayerMask.GetMask("Unit", "Building", "Resource", "Ground");
    }
    
    void Update()
    {
        // mouse up
        if (Input.GetMouseButtonUp(1))
        {
            TryCommand(Input.mousePosition);
        }
    }
    
    private void UnitsMoveToPosition(Vector3 dest, Unit unit)
    {
        if (unit)
            unit.MoveToPosition(dest);
    }

    private void CommandToGround(RaycastHit hit, Unit unit)
    {
        UnitsMoveToPosition(hit.point, unit);
        CreateVFXMarker(hit.point, MainUI.Instance.SelectionMarker);
    }
    
    private void TryCommand(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        //if we left-click something
        if (!Physics.Raycast(ray, out hit, 1000, layerMask)) return;
        switch (hit.collider.tag)
        {
            case "Ground":
                CommandToGround(hit, unitSelect.CurUnit);
                break;
            case "Resource":
                ResourceCommand(hit, unitSelect.CurUnit);
                break;
        }
    }
    
    private void CreateVFXMarker(Vector3 pos, GameObject vfxPrefab)
    {
        if (!vfxPrefab)
            return;

        Instantiate(vfxPrefab, new Vector3(pos.x, 0.1f, pos.z), Quaternion.identity);
    }
    
    private void UnitsToGatherResource(ResourceSource resource, Unit unit)
    {
        if (unit.IsWorker)
            unit.Worker.ToGatherResource(resource, resource.transform.position);
        else
            unit.MoveToPosition(resource.transform.position);
    }
    
    private void ResourceCommand(RaycastHit hit, Unit unit)
    {
        UnitsToGatherResource(hit.collider.GetComponent<ResourceSource>(), unit);
        CreateVFXMarker(hit.transform.position, MainUI.Instance.SelectionMarker);
    }
}
