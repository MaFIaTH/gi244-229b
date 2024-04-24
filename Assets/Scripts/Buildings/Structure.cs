using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StructureCost
{
    public int food;
    public int wood;
    public int gold;
    public int stone;
}
public abstract class Structure : MonoBehaviour
{
    [SerializeField] protected string structureName;
    [SerializeField] protected Sprite structurePic;
    [SerializeField] protected int baseMaxHP = 100;
    [SerializeField] protected int curHP;
    [SerializeField] protected int maxHP;
    [SerializeField] protected Faction faction;
    [SerializeField] protected GameObject selectionVisual;
    [SerializeField] protected StructureCost structureCost;
    
    public string StructureName => structureName;
    public Sprite StructurePic => structurePic;

    
    public int BaseMaxHP
    {
        get => baseMaxHP;
        set => baseMaxHP = value;
    }
    public int CurHP
    {
        get => curHP;
        set => curHP = value;
    }

    public int MaxHP
    {
        get => maxHP;
        set => maxHP = value;
    }

    public GameObject SelectionVisual => selectionVisual;
    public Faction Faction 
    {
        get => faction;
        set => faction = value;
    }

    public StructureCost StructureCost
    {
        get => structureCost;
        set => structureCost = value;
    }
    
    public void TakeDamage(int damage)
    {
        curHP -= damage;
        if (curHP <= 0)
            Die();
    }

    
    protected virtual void Die()
    {
        InfoManager.Instance.ClearAllInfo();
        Destroy(gameObject);
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
