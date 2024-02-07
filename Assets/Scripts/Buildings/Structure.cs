using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    [SerializeField] protected string structureName;
    [SerializeField] protected Sprite structurePic;
    [SerializeField] protected int curHP;
    [SerializeField] protected int maxHP;
    [SerializeField] protected Faction faction;
    [SerializeField] protected GameObject selectionVisual;
    
    public string StructureName => structureName;
    public Sprite StructurePic => structurePic;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
