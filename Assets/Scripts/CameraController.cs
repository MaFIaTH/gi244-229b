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
    }

    // Update is called once per frame
    void Update()
    {
        MoveByKeyboard();
    }

    private void MoveByKeyboard()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        Vector3 dir = (transform.forward * zInput) + (transform.right * xInput);
        gameObject.transform.position += dir * (moveSpeed * Time.deltaTime);
    }
}
