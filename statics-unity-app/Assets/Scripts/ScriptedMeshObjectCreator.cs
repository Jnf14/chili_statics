using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScriptedMeshObjectCreator : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField]
    private List<Vector3> vertices;
    [SerializeField]
    private List<int> triangles;
    private List<Vector2> circleUV = new List<Vector2>
        {
            new Vector2(0.9720596f, 0.6532523f),
            new Vector2(0.9015915f, 0.7915539f),
            new Vector2(0.7918347f, 0.9013107f),
            new Vector2(0.653533f, 0.971779f),
            new Vector2(0.5002243f, 0.9960608f),
            new Vector2(0.3469157f, 0.971779f),
            new Vector2(0.2086142f, 0.9013106f),
            new Vector2(0.0988576f, 0.7915537f),
            new Vector2(0.0283896f, 0.6532522f),
            new Vector2(0.004108027f, 0.4999439f),
            new Vector2(0.02838971f, 0.3466356f),
            new Vector2(0.09885782f, 0.2083341f),
            new Vector2(0.2086144f, 0.0985775f),
            new Vector2(0.3469159f, 0.02810938f),
            new Vector2(0.5002242f, 0.003827723f),
            new Vector2(0.6535327f, 0.02810941f),
            new Vector2(0.7918344f, 0.09857755f),
            new Vector2(0.901591f, 0.2083343f),
            new Vector2(0.9720591f, 0.3466357f),
            new Vector2(0.9963408f, 0.4999439f)
        };

    [SerializeField]
    private float angle;
    [SerializeField]
    private float startRadius;
    [SerializeField]
    private float endRadius;
    [SerializeField]
    private GameObject startSnow;
    [SerializeField]
    private GameObject endSnow;

    private readonly int iMax = 19;

    private void SetVertices()
    {
        float power = 3f;
        vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        vertices.Add(new Vector3(0, -1, 0));
        uv.Add(new Vector2(0.5f, 0.5f));
        for (int i = 0; i <= iMax; i++)
        {
            vertices.Add(new Vector3((float)Math.Cos(Math.PI * angle * i / (iMax * 180)) * (startRadius + (endRadius - startRadius) * (float)Math.Pow(Math.Sin(Math.PI / 2 * i / iMax), power)), -1, (float)Math.Sin(Math.PI * angle * i / (iMax * 180)) * (startRadius + (endRadius - startRadius) * (float)Math.Pow(Math.Sin(Math.PI / 2 * i / iMax), power))));
            uv.Add(circleUV[i]);
        }
        vertices.Add(new Vector3(0, 1, 0));
        uv.Add(new Vector2(0.5f, 0.5f));
        for (int i = 0; i <= iMax; i++)
        {
            vertices.Add(new Vector3((float)Math.Cos(Math.PI * angle * i / (iMax * 180)) * (startRadius + (endRadius - startRadius) * (float)Math.Pow(Math.Sin(Math.PI / 2 * i / iMax), power)), 1, (float)Math.Sin(Math.PI * angle * i / (iMax * 180)) * (startRadius + (endRadius - startRadius) * (float)Math.Pow(Math.Sin(Math.PI / 2 * i / iMax), power))));
            uv.Add(circleUV[i]);
        }
        
        for (int i = 0; i <= iMax; i++)
        {
            vertices.Add(new Vector3((float)Math.Cos(Math.PI * angle * i / (iMax * 180)) * (startRadius + (endRadius - startRadius) * (float)Math.Pow(Math.Sin(Math.PI / 2 * i / iMax), power)), -1, (float)Math.Sin(Math.PI * angle * i / (iMax * 180)) * (startRadius + (endRadius - startRadius) * (float)Math.Pow(Math.Sin(Math.PI / 2 * i / iMax), power))));
            uv.Add(new Vector2(0, (float)i / iMax));
            //uv.Add(circleUV[i]);
        }
        for (int i = 0; i <= iMax; i++)
        {
            vertices.Add(new Vector3((float)Math.Cos(Math.PI * angle * i / (iMax * 180)) * (startRadius + (endRadius - startRadius) * (float)Math.Pow(Math.Sin(Math.PI / 2 * i / iMax), power)), 1, (float)Math.Sin(Math.PI * angle * i / (iMax * 180)) * (startRadius + (endRadius - startRadius) * (float)Math.Pow(Math.Sin(Math.PI / 2 * i / iMax), power))));
            uv.Add(new Vector2(1, (float)i / iMax));
            //uv.Add(circleUV[i]);
        }
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
    }

    private void SetTriangles()
    {
        triangles = new List<int>();
        for (int i = 1; i <= iMax; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }
        for (int i = iMax + 1; i <= 2 * iMax + 2; i++)
        {
            triangles.Add(21);
            triangles.Add(i + 1);
            triangles.Add(i);
        }
        for (int i = 2 * iMax + 4; i < vertices.Count - iMax - 2; i++)
        {
            triangles.Add(i);
            triangles.Add(i + iMax + 1);
            triangles.Add(i + 1);

            triangles.Add(i + 1);
            triangles.Add(i + iMax + 1);
            triangles.Add(i + iMax + 2);
        }
        mesh.triangles = triangles.ToArray();
    }

    private void Start()
    {
        //copied from unity cylinder
        Recalculate();
    }

    public void Recalculate()
    {
        startRadius = startSnow.transform.localScale.x;
        endRadius = endSnow.transform.localScale.x;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        SetVertices();
        SetTriangles();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }
}
