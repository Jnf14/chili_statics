using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Attachment attachment;

    private void Awake()
    {
        attachment = gameObject.GetComponent<Attachment>();
    }

    public void Move(float[] coordinates, float rotation)
    {
        Vector3 oldPosition = transform.position;
        Vector3 newPosition = new Vector3(coordinates[0], transform.position.y, coordinates[1]);
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, rotation, transform.eulerAngles.z);
        if (attachment == null || attachment.target == null)
        {
            transform.rotation = Quaternion.Euler(newRotation);
            transform.position = newPosition;
        }
        else
        {
            attachment.ProjectOnTarget(newPosition);
        }

        // GameManager.instance.ExportLog("load", "move", "Moved " + attachment.pointLoad.typeText + " " + attachment.pointLoad.id + " from " + oldPosition.ToString() + " to " + newPosition.ToString());

    }
}
