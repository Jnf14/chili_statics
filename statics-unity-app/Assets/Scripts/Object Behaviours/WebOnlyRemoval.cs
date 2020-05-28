using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebOnlyRemoval : MonoBehaviour
{
    [SerializeField]
    private float deleteLine;
    private Plane[] planes;
    private Collider objectCollider;
    private Removal removal;

    private void Start()
    {
        deleteLine = 30 * Camera.main.pixelHeight / 400;
        objectCollider = GetComponent<Collider>();
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        removal = GetComponent<Removal>();
    }

    private void Update()
    {
        if (!GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds))
        {
            removal.Remove();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            removal.Remove();
        }
        if (Input.GetMouseButtonUp(0) && InDeleteZone())
        {
            removal.Remove();
        }
    }

    private void OnDestroy()
    {
        int id = int.Parse(gameObject.name.Substring(7));
        ObjectManager.instance.WebDeleteDone(id);
    }

    private bool InDeleteZone()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        return (pos.y <= deleteLine);
    }
}
