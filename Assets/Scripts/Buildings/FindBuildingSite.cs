using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBuildingSite : MonoBehaviour
{
    [SerializeField] private bool canBuild = false;
    [SerializeField] private MeshRenderer[] modelRdr;
    [SerializeField] private MeshRenderer planeRdr;

    public bool CanBuild
    {
        get => canBuild;
        set => canBuild = value;
    }

    private void Start()
    {
        //Setup Building Color
        foreach (var t in modelRdr)
            t.material.color = Color.green;

        //Setup Plane Color
        planeRdr.material.color = Color.green;
        
        CanBuild = true;
    }
    
    private void SetCanBuild(bool flag)
    {
        if (flag)
        {
            foreach (var t in modelRdr)
                t.material.color = Color.green;

            planeRdr.material.color = Color.green;
            canBuild = true;
        }
        else
        {
            foreach (var t in modelRdr)
                t.material.color = Color.red;

            planeRdr.material.color = Color.red;
            canBuild = false;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        CheckCanBuild(other);
    }

    void OnTriggerStay(Collider other)
    {
        CheckCanBuild(other);
    }

    void OnTriggerExit(Collider other)
    {
        CheckCanBuild(other);
    }

    private void CheckCanBuild(Collider other)
    {
        if (other.CompareTag("Resource") || other.CompareTag("Building") || other.CompareTag("Unit"))
            SetCanBuild(false);
        else 
            SetCanBuild(true);
    }


}
