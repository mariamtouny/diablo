using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes the camera follow the player

public class CameraController : MonoBehaviour
{

    public Transform target;    

    public Vector3 offset = new Vector3 (0, -1.17999995f, -0.790000021f);          

    public float pitch = 1.5f;        

    public float yawSpeed = 100f;   

    private float currentZoom = 15f;
    private float currentYaw = 0f;

    private void Start()
    {
        
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(target);
    }

    void Update()
    {
        // Adjust our zoom based on the scrollwheel
        //currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        //currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // Adjust our camera's rotation around the player
        //currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;

        transform.LookAt(target.position + Vector3.up * pitch);

        //transform.RotateAround(target.position, Vector3.up, currentYaw);
    }

}