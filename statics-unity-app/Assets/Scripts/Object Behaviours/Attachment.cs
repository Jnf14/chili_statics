using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    public Collider target = null;
    public float displacement;
    public float distanceFromEnds;
    public float threshold;
    private ElementNodes elementNodes;
    public PointLoad pointLoad;
    public List<Collider> hitColliders = new List<Collider>();

    private void Awake()
    {
        pointLoad = gameObject.GetComponent<PointLoad>();
    }

    private void Update()
    {
        if (hitColliders.Count != 0)
        {
            Attach(hitColliders[0]);
            hitColliders.Clear();
        }
    }

    public void Attach(Collider target)
    {
        if (this.target != null)
        {
            return;
        }
        this.target = target;
        elementNodes = target.GetComponent<ElementNodes>();
        pointLoad.SetPointLoad(elementNodes, int.Parse(target.name.Substring(8)), target.transform.eulerAngles.y, displacement);
        GameManager.AddPointLoad(pointLoad);
        ProjectOnTarget(transform.position);
        // GameManager.instance.ExportLog("load", "attach", "Attached load " + pointLoad.id + " (" + pointLoad.typeText + ") on beam " + pointLoad.beamId + " at " + pointLoad.positionFromLeftEnd.ToString("0.0") + "%");
    }

    public void Detach()
    {
        if (target == null)
        {
            return;
        }
        target = null;
        GetComponent<PointLoad>().Detach();
        GameManager.RemovePointLoad(pointLoad);
        // GameManager.instance.ExportLog("load", "detach", "Detached load " + pointLoad.id + " (" + pointLoad.typeText + ") from beam " + pointLoad.beamId);
    }

    public void ProjectOnTarget(Vector3 position)
    {
        if (target == null)
        {
            return;
        }

        Vector3 point1 = elementNodes.n1.transform.position;
        Vector3 point2 = elementNodes.n2.transform.position;
        point1 = point1 + distanceFromEnds * Vector3.Normalize(point2 - point1);
        point2= point2 + distanceFromEnds * Vector3.Normalize(point1 - point2);

        Vector3 directionVector = Vector3.Normalize(point2 - point1);
        Vector3 normalVector = new Vector3(-directionVector.z, 0, directionVector.x);
        Vector3 newPosition = new Vector3(0, 0, 0);
        newPosition += point1;
        newPosition += Vector3.Dot(position - point1, directionVector) * directionVector;
        
        if (Vector3.Dot(point2 - point1, newPosition - point1) < 0)
        {
            newPosition = point1;
        }
        if (Vector3.Dot(point1 - point2, newPosition - point2) < 0)
        {
            newPosition = point2;
        }

        if (Vector3.Distance(newPosition, position) > threshold)
        {
            Detach();
            return;
        }

        newPosition += displacement * normalVector;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Vector3.SignedAngle(Vector3.right, directionVector, Vector3.up), transform.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null)
        {
            return;
        }
        hitColliders.Add(other);
    }
}
