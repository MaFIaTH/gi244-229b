using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Nations
{
    Neutral = 0,
    Britain,
    Pirates,
    France,
    Spain,
    Portugal,
    Netherlands
}
public class Faction : MonoBehaviour
{
    [SerializeField] private Nations nations;
    
    [Header("Resources")]
    [SerializeField] private int food;
    [SerializeField] private int wood;
    [SerializeField] private int gold;
    [SerializeField] private int stone;
    
    public int Food { get => food; set => food = value; }
    public int Wood { get => wood; set => wood = value; }
    public int Gold { get => gold; set => gold = value; }
    public int Stone { get => stone; set => stone = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
