using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Removal : MonoBehaviour
{
    Attachment attachment;

    private void Awake()
    {
        attachment = GetComponent<Attachment>();
    }

    public void Remove()
    {
        if (attachment != null)
        {
            attachment.Detach();
        }
        Destroy(gameObject);

        GameManager.instance.ExportLog("point load", "remove", "Removed " + attachment.pointLoad.typeText + " from scene" );    
    }
}
