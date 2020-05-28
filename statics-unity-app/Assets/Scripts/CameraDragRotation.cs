using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragRotation : MonoBehaviour
{
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private Vector3 positionVelocity = Vector3.zero;
    private Vector3 rotationVelocity = Vector3.zero;
    private float smoothTime = 0.3f;

    private readonly Vector3 centerPoint = new Vector3(0, 0.4f, 0);
    private readonly float dragSensitivity = 0.18f;
    private Vector3 dragOrigin;
    [SerializeField]
    private bool draggingMode = false;
    [SerializeField]
    private float horizontalRotation = 0;
    [SerializeField]
    private float verticalRotation = 0;

    public bool Dragging
    {
        get
        {
            return draggingMode;
        }

        set
        {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
            horizontalRotation = 0;
            verticalRotation = 0;
            draggingMode = value;
        }
    }

    private void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    private void Update()
    {
        if (!draggingMode)
        {
            //Smooth camera to default position?
            //find formula for horizontal and vertical rotation relative to the position of the camera

            //transform.position = Vector3.SmoothDamp(transform.position, defaultPosition, ref positionVelocity, smoothTime);
            //transform.rotation = Quaternion.LookRotation(centerPoint - transform.position, Vector3.forward);
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {

            if (System.Math.Abs(horizontalRotation + (Input.mousePosition.x - dragOrigin.x) * dragSensitivity) < 89)
            {
                gameObject.transform.RotateAround(centerPoint, Vector3.forward, (Input.mousePosition.x - dragOrigin.x) * dragSensitivity);
                horizontalRotation += (Input.mousePosition.x - dragOrigin.x) * dragSensitivity;
            }
            
            
            if (System.Math.Abs(verticalRotation + (Input.mousePosition.y - dragOrigin.y) * dragSensitivity) < 89)
            {
                Vector3 axis = new Vector3(-(float)System.Math.Cos(horizontalRotation * System.Math.PI / 180f), -(float)System.Math.Sin(horizontalRotation * System.Math.PI / 180f), 0);
                gameObject.transform.RotateAround(centerPoint, axis, (Input.mousePosition.y - dragOrigin.y) * dragSensitivity);
                verticalRotation += (Input.mousePosition.y - dragOrigin.y) * dragSensitivity;
            }

            dragOrigin = Input.mousePosition;
        }
    }
}
