using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Obstacle : MonoBehaviour
{
    public Cube middle;
    public Cube sz;
    public Cube sp;
    public Vertex editorVertexPrefab;
    
    public Vertex[] vertex = new Vertex[6];
    // Start is called before the first frame update
    void Awake()
    {
        var middlesp = Random.Range(0.4f, 0.8f);
        var middlesz = Random.Range(0.4f, 0.8f);
        var sz1 = Random.Range(0.4f, 0.8f);
        var sp1 = Random.Range(0.4f, 0.8f);
        
        // use the center points to build the obstacle
        // position = length/2
        middle.transform.localPosition = new Vector3(-middlesp / 2, -middlesz / 2, 0);
        middle.transform.localScale = new Vector3(middlesp, middlesz, 1);
        sz.transform.localPosition = new Vector3(-middlesp / 2, sz1 / 2, 0);
        sz.transform.localScale = new Vector3(middlesp, sz1, 1);
        sp.transform.localPosition = new Vector3(sp1 / 2, -middlesz / 2, 0);
        sp.transform.localScale = new Vector3(sp1, middlesz, 1);

        int[] degs = new int[] { 0, 90, 180, 270 };
        int i = Random.Range(0, 4);
        gameObject.transform.Rotate(0, 0, degs[i]);

        var r1 = changePositionZ(sz.TopRight);
        var r2 = changePositionZ(sz.TopLeft);
        var r3 = changePositionZ(middle.BottomLeft);
        var r4 = changePositionZ(sp.BottomRight);
        var r5 = changePositionZ(sp.TopRight);
        var notReflex = changePositionZ(middle.TopRight);
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(r1);
        vertices.Add(r2);
        vertices.Add(r3);
        vertices.Add(r4);
        vertices.Add(r5);
        vertices.Add(notReflex);
        for (int j = 0; j < 6; j++)
        {
            vertex[j] = Instantiate(editorVertexPrefab, vertices[j], Quaternion.identity);
            if (j < 5)
            {
                vertex[j].isReflex = true;
            }
            else
            {
                vertex[j].isReflex = false;
            }
        }
        for (int j = 0; j < 6; j++)
        {
            if (j == 0)
            {
                vertex[j].prev = vertex[vertices.Count - 1];
                vertex[j].next = vertex[j + 1];


            }
            else if (j == vertices.Count - 1)
            {
                vertex[j].prev = vertex[j - 1];
                vertex[j].next = vertex[0];
            }
            else
            {
                vertex[j].prev = vertex[j - 1];
                vertex[j].next = vertex[j + 1];
            }
        }


            }
    Vector3 changePositionZ(Vector3 a)
    {
        Vector3 changeZ = new Vector3(a.x, a.y, Helper.ZValueOfVertex);
        return changeZ;



    }
}
