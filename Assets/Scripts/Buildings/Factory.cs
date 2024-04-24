using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    [SerializeField] private StructureCost resourceProduction;
    [SerializeField] private float productionTime;
    [SerializeField] private bool isFactory = true;
    
    public bool IsFactory => isFactory;
    
    private float _productionTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsFunctional) return;
        _productionTimer -= Time.deltaTime;
        _productionTimer = Mathf.Max(_productionTimer, 0);
        if (_productionTimer > 0) return;
        ProduceResource();
    }
    
    private void ProduceResource()
    {
        _productionTimer = productionTime;
        Faction.GainResource(ResourceType.Food, resourceProduction.food);
        Faction.GainResource(ResourceType.Wood, resourceProduction.wood);
        Faction.GainResource(ResourceType.Gold, resourceProduction.gold);
        Faction.GainResource(ResourceType.Stone, resourceProduction.stone);
    }
}
