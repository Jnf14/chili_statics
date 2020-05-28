using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragging : MonoBehaviour
{
    private Movement movement;
    private Vector3 offset;
    private float distanceFromCamera;
    
    private void Start()
    {
        movement = GetComponent<Movement>();
        distanceFromCamera = System.Math.Abs(transform.position.y - Camera.main.transform.position.y);
    }

    private void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));
        offset = transform.position - mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));
        movement.Move(new float[] { (mousePosition + offset).x, (mousePosition + offset).z }, 0);
    }
}
