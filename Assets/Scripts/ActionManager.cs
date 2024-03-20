using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    [SerializeField] private Button[] unitBtns;
    [SerializeField] private Button[] buildingBtns;

    private CanvasGroup cg;
    

    private void Awake()
    {
        Instance = this;
        cg = GetComponent<CanvasGroup>();
    }

    private void HideCreateUnitButtons()
    {
        foreach (var t in unitBtns)
            t.gameObject.SetActive(false);
    }
    private void HideCreateBuildingButtons()
    {
        foreach (var t in buildingBtns)
            t.gameObject.SetActive(false);
    }
    public void ClearAllInfo()
    {
        HideCreateUnitButtons();
        HideCreateBuildingButtons();
    }
    private void ShowCreateUnitButtons(Building b)
    {
        if (!b.IsFunctional) return;
        for (int i = 0; i < b.UnitPrefabs.Length; i++)
        {
            unitBtns[i].gameObject.SetActive(true);
            Unit unit = b.UnitPrefabs[i].GetComponent<Unit>();
            unitBtns[i].image.sprite = unit.UnitPic;
        }
    }
    
    private void ShowCreateBuildingButtons(Unit u) //Showing list of buildings when selecting a single unit
    {
        if (!u.IsBuilder) return;
        for (int i = 0; i < u.Builder.BuildingList.Length; i++)
        {
            buildingBtns[i].gameObject.SetActive(true);

            if (u.Builder.BuildingList[i] != null)
            {
                buildingBtns[i].GetComponent<Button>().interactable = true;
                buildingBtns[i].image.color = Color.white;
                Building building = u.Builder.BuildingList[i].GetComponent<Building>();
                buildingBtns[i].image.sprite = building.StructurePic;
            }
            else
            {
                buildingBtns[i].GetComponent<Button>().interactable = false;
                buildingBtns[i].image.color = Color.clear;
            }
        }
    }
    public void ShowCreateUnitMode(Building b)
    {
        ClearAllInfo();
        ShowCreateUnitButtons(b);
    }

    public void ShowBuilderMode(Unit unit)
    {
        ClearAllInfo();
        ShowCreateBuildingButtons(unit);
    }
    public void CreateUnitButton(int n)//Map with Create Unit Btns
    {
        //Debug.Log("Create " + n);
        UnitSelect.Instance.CurBuilding.ToCreateUnit(n);
    }

    public void CreateBuildingButton(int n)//Map with Create Building Btns
    {
        //Debug.Log("1 - Click Button: " + n);
        Unit unit = UnitSelect.Instance.CurUnits[0];
        
        if (!unit.IsBuilder) return;
        unit.Builder.ToCreateNewBuilding(n);
    }

}
