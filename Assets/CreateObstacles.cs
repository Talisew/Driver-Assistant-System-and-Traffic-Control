using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateObstacles : MonoBehaviour
{
    // the area of the center rectangular

    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;
    public Vector3[] center = new Vector3[4];
    
    //pick the center for the three obstacles, since the obstacles if from 0.2f tp 0.5f
    // so area -0.6f(>0.5f) at four side
    public Vector3[] pickCenter3()
    {
        Vector3[] center = new Vector3[3];
        var x1 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
        var y1= Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
        var z1 = 2.0f;
        Vector3 a = new Vector3(x1, y1, -2.0f);
        center[0] = a;
        var x2 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
        var y2 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
        Vector3 b = new Vector3(x2, y2, -2.0f);
        center[1] = b;
        while (distanceCheck(a,b) == 0)
        {
            x2 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
            y2 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
            b = new Vector3(x2, y2, -2.0f);
            center[1] = b;
        }
        var x3 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
        var y3 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
        Vector3 c = new Vector3(x3, y3, -2.0f);
        center[2] = c;
        while (distanceCheck(a, c) == 0 || distanceCheck(b,c)==0)
        {
            x3 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
            y3 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
            c = new Vector3(x3, y3, -2.0f);
            center[2] = c;
        }
        return center;
    }
    public Vector3[] pickCenter4()
    {
        Vector3[] center = new Vector3[4];
        var x1 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
        var y1 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
        var z1 = 2.0f;
        Vector3 a = new Vector3(x1, y1, -2.0f);
        center[0] = a;
        var x2 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
        var y2 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
        Vector3 b = new Vector3(x2, y2, -2.0f);
        center[1] = b;
        while (distanceCheck(a, b) == 0)
        {
            x2 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
            y2 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
            b = new Vector3(x2, y2, -2.0f);
            center[1] = b;
        }
        var x3 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
        var y3 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
        Vector3 c = new Vector3(x3, y3, -2.0f);
        center[2] = c;
        while (distanceCheck(a, c) == 0 || distanceCheck(b, c) == 0)
        {
            x3 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
            y3 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
            c = new Vector3(x3, y3, -2.0f);
            center[2] = c;
        }
        var x4 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
        var y4 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
        Vector3 d = new Vector3(x3, y3, -2.0f);
        center[3] = d;
        while (distanceCheck(a, d) == 0 || distanceCheck(b, d) == 0|| distanceCheck(c,d) == 0)
        {
            x4 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 0.6f);
            y4 = Random.Range(bottomRight.position.y + 0.6f, topRight.position.y - 0.6f);
            d = new Vector3(x4, y4, -2.0f);
            center[3] = d;
        }
        return center;
    }
    int distanceCheck(Vector3 a, Vector3 b)
    {
        int i = 0;
        if(Mathf.Abs(a.x-b.x)>=4f|| Mathf.Abs(a.y - b.y) >= 4f)
        {
            i = 1;
        }
        return i;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
