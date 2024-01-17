using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    [Header("Move")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float xInput;
    [SerializeField] private float zInput;
    [SerializeField] private Transform corner1, corner2;
    
    [Header("Zoom")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomModifier;

    [SerializeField] private float minZoomDist;
    [SerializeField] private float maxZoomDist;

    [SerializeField] private float dist;
    

    public static CameraController Instance;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 50f;
        zoomSpeed = 25f;
        minZoomDist = 15f;
        maxZoomDist = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveByKeyboard();
        Zoom();
    }

    private void MoveByKeyboard()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        Vector3 dir = (transform.forward * zInput) + (transform.right * xInput);
        transform.position += dir * (moveSpeed * Time.deltaTime);
        transform.position = ClampCamera(corner1.position, corner2.position);
    }

    private Vector3 ClampCamera(Vector3 lowerLeft, Vector3 topRight)
    {
        Vector3 pos = new Vector3(Mathf.Clamp(transform.position.x, lowerLeft.x, topRight.x), transform.position.y,
            Math.Clamp(transform.position.z, lowerLeft.z, topRight.z));
        return pos;
    }
    
    private void Zoom()
    {
        zoomModifier = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKey(KeyCode.Z))
            zoomModifier = 0.01f;
        if (Input.GetKey(KeyCode.X))
            zoomModifier = -0.01f;

        dist = Vector3.Distance(transform.position, cam.transform.position);

        if (dist < minZoomDist && zoomModifier > 0f)
            return; //too close
        else if (dist > maxZoomDist && zoomModifier < 0f)
            return; //too far

        cam.transform.position += cam.transform.forward * zoomModifier * zoomSpeed;
    }

}
