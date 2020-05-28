using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    public string Text
    {
        set
        {
            text.text = value;
        }
    }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position + new Vector3(transform.position.x, 0, transform.position.z), Camera.main.transform.up);
        transform.Rotate(180, 0, 180);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
