using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVG
{
    public readonly List<Vertex> vertices = new List<Vertex>();
    public readonly List<(Vertex, Vertex)> edges = new List<(Vertex, Vertex)>();
    public RVG(List<Vertex> allVertices)
    {
        // store all nodes in a list
        foreach (var vertex in allVertices)
        {
            if (vertex.isReflex)
            {
                vertices.Add(vertex);
            }
        }
        
        for (int i = 0; i < vertices.Count; i++)
        {
            for (int j = i + 1; j < vertices.Count; j++)
            {
                Vector3 node1 = new Vector3(vertices[i].Position.x,vertices[i].Position.y,Helper.ZValueOfObstacle);
                Vector3 node2 = new Vector3(vertices[j].Position.x, vertices[j].Position.y, Helper.ZValueOfObstacle);

                var dir = node1 - node2;
                // I will use the raycast to detect whether there's an obstacle between the two node.
                //Debug.DrawRay(node1, dir,Color.yellow,1000);
                if (!Physics.Raycast(node2, dir, dir.magnitude))
                {
                    
                    if (checkBiTan(vertices[i], vertices[j]))
                    {
                        edges.Add((vertices[i], vertices[j]));
                    }
                }
            }
        }
        // this one including all the vertrices and distance of them this vertex can connect
        foreach (var (a, b) in edges)
        {
            float distance = (b.Position - a.Position).magnitude;
            a.neighbors.Add((b, distance));
            b.neighbors.Add((a, distance));
        }
    }



    bool checkBiTan(Vertex a,Vertex b)
    {
        var dir1 = colinear(a.next.Position, a.Position, b.Position);
        var dir2 = colinear(a.prev.Position, a.Position, b.Position);
        if ((dir1 != dir2)&& dir1>0 && dir2>0) {
            return false;
        }

        // check if b is good

        dir1 = colinear(b.next.Position, b.Position, a.Position);
        dir2 = colinear(b.prev.Position, b.Position, a.Position);
        if ((dir1 != dir2) && dir1 > 0 && dir2 > 0) {
            return false;
         }
        // both are good
        return true;

    }
    int colinear(Vector3 a, Vector3 b, Vector3 c)
    {
        int i = 0;
        var angle = (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
        if (angle > 0)
        {
            i = 1;
        }else if (angle < 0)
        {
            i = 2;
        }
        return i;
    }

    
}
