using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapBlip : MonoBehaviour
{
    private GameObject blip;
    private Unit unit;
    private Building building;
    private Faction faction;
    
    public GameObject Blip => blip;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        building = GetComponent<Building>();
    }

    // Start is called before the first frame update
    void Start()
    {
        blip = Instantiate(MiniMap.Instance.blipPrefab, MiniMap.Instance.blipParent.transform);
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        blip.transform.position = MiniMap.Instance.worldPosToMinimapPos(transform.position);
        blip.transform.position = MainUI.Instance.ScalePosition(blip.transform.position);
    }
    
    private void SetColor()
    {
        if (unit != null)
            faction = unit.Faction;

        if (building != null)
        {
            faction = building.Faction;
            blip.GetComponent<RectTransform>().sizeDelta = new Vector2(6f, 6f);
        }

        if (faction != null)
            blip.GetComponent<Image>().color = faction.GetNationColor();
        else
            blip.GetComponent<Image>().color = Color.white;
    }
    
    void OnDestroy()
    {
        Destroy(blip);
    }
}
