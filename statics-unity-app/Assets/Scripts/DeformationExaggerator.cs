using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationExaggerator : MonoBehaviour
{
    [SerializeField]
    private Vector3[] positions;
    private LineRenderer lineRenderer;
    [SerializeField]
    private int position;

    public void Recalculate()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.positionCount = positions.Length;
        Vector3[] points = new Vector3[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 vector = 0.006f * (GameManager.instance.starters[position][i] - DeformationManager.instance.Exagg * (GameManager.instance.starters[position][i] - positions[i]));
            points[i] = new Vector3(vector.x, 2, vector.y - 7);
        }
        lineRenderer.SetPositions(points);
        lineRenderer.numCapVertices = 50;
    }

    public void SetPositions(List<Vector3> positions)
    {
        this.positions = positions.ToArray();
        Recalculate();
    }
}
