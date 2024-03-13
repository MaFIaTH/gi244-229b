using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] woodTreePrefab;

    [SerializeField]
    private Transform woodTreeParent;

    [SerializeField]
    private ResourceSource[] resources;

    public static ResourceManager Instance;

    public ResourceSource[] Resources => resources;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        FindAllResource();
    }
    
    private void FindAllResource()
    {
        resources = FindObjectsOfType<ResourceSource>();
    }
    
    

}
