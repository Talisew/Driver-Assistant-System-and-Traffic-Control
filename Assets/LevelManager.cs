using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;
    public Obstacle obstaclePrefab;
    public GameObject pointPrefab;
    public Vertex vertexPrefab;
    public Line linePrefab;
    //public Transform reflexVers;
    //public Transform verticesForBasics;
    public readonly List<Vertex[]> shapes = new List<Vertex[]>();
    public readonly List<Vector3> reflex = new List<Vector3>();

    public Transform floorVertices;
    public readonly List<Vertex> editorVertices = new List<Vertex>();
    public RVG rvg { get; private set; }
    // to know when the buildRVG finish
    
    //store the obstacle
    //public List<Obstacle> obstacles = new List<Obstacle>();
    // Start is called before the first frame update
    void Start()
    {
        PathFinding.tmpVertex1 = Instantiate(vertexPrefab);
        PathFinding.tmpVertex2 = Instantiate(vertexPrefab);

        runObstacles();

        var a = LinkVerticesOfShape(floorVertices);
        editorVertices.AddRange(a);

        // test if prev next pointers are set correctly
        //foreach (var v in editorVertices)
        //{
            //var line = Instantiate(linePrefab);
            //line.SetVertices(v.Position, v.next.Position);
        //}

        // give enough time for ray to detect the obstacles
        Invoke(nameof(buildRVG), 0.1f);
        // print the result for the four variables after 60 seconds from starting running
        Invoke(nameof(printResult), 60f);

    }
    // use this method to show the results stored in the Helper
    public void printResult()
    {
        Debug.Log($"Number of paths planned: {Helper.numOfPath}");
        Debug.Log($"Number of replannings: {Helper.numOfReplan}");
        Debug.Log($"Number of plans successfully reached: {Helper.numOfSuccessReach}");
        Debug.Log($"Total planning time (ms): {Helper.totalPlanningTime.TotalMilliseconds}");
    }

    // link all child vertices of transform in a circular array like way
    public List<Vertex> LinkVerticesOfShape(Transform parent)
    {
        List<Vertex> vertices = new List<Vertex>(parent.childCount);
        foreach (Transform child in parent)
        {
            vertices.Add(child.gameObject.GetComponent<Vertex>());
        }
        for (int i = 0; i < vertices.Count; i++)
        {
            Vertex cur = vertices[i];
            int prevIdx = i - 1;
            int nextIdx = i + 1;
            if (prevIdx < 0) { prevIdx = vertices.Count - 1; };
            if (nextIdx == vertices.Count) { nextIdx = 0; };

            cur.next = vertices[nextIdx];
            cur.prev = vertices[prevIdx];
        }
        return vertices;
    }
    public List<Vector3> storePts(Transform pts)
    {
        //store each point in the pts variable
        var store = new List<Vector3>();
        foreach (Transform point in pts)
        {
            store.Add(point.position);
        }
        return store;
    }
    
    //pick the center for the three obstacles, since the obstacles if from 0.2f tp 0.5f
    // so area -1f(>） at four side
    public Vector3[] pickCenter3()
    {
        //Debug.Log(topLeft.position);
        Vector3[] center = new Vector3[3];
        var x1 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
        var y1 = Random.Range(bottomRight.position.y + 1, topRight.position.y - 1f);
        Vector3 a = new Vector3(x1, y1, -2.0f);
        center[0] = a;
        var x2 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
        var y2 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
        Vector3 b = new Vector3(x2, y2, -2.0f);
        center[1] = b;

        Helper.NoInfiniteLoopStart();
        while (IsOverlapping(a, b) && Helper.NoInfiniteLoop)
        {
            x2 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
            y2 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
            b = new Vector3(x2, y2, -2.0f);
            center[1] = b;
        }
        var x3 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
        var y3 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
        Vector3 c = new Vector3(x3, y3, -2.0f);
        center[2] = c;

        Helper.NoInfiniteLoopStart();
        while ((IsOverlapping(a, c) || IsOverlapping(b, c)) && Helper.NoInfiniteLoop)
        {
            x3 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
            y3 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
            c = new Vector3(x3, y3, -2.0f);
            center[2] = c;
        }
        //Debug.Log(center[0]);
        return center;
    }
    public Vector3[] pickCenter4()
    {
        Vector3[] center = new Vector3[4];
        var x1 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
        var y1 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
        Vector3 a = new Vector3(x1, y1, Helper.ZValueOfObstacle);
        center[0] = a;
        var x2 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
        var y2 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
        Vector3 b = new Vector3(x2, y2, Helper.ZValueOfObstacle);
        center[1] = b;

        Helper.NoInfiniteLoopStart();
        while (IsOverlapping(a, b) && Helper.NoInfiniteLoop)
        {
            x2 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
            y2 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
            b = new Vector3(x2, y2, Helper.ZValueOfObstacle);
            center[1] = b;
        }
        var x3 = Random.Range(topLeft.position.x + 0.6f, topRight.position.x - 1f);
        var y3 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
        Vector3 c = new Vector3(x3, y3, Helper.ZValueOfObstacle);
        center[2] = c;

        Helper.NoInfiniteLoopStart();
        while ((IsOverlapping(a, c) || IsOverlapping(b, c)) && Helper.NoInfiniteLoop)
        {
            x3 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
            y3 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
            c = new Vector3(x3, y3, Helper.ZValueOfObstacle);
            center[2] = c;
        }
        var x4 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
        var y4 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
        Vector3 d = new Vector3(x4, y4, Helper.ZValueOfObstacle);
        center[3] = d;

        Helper.NoInfiniteLoopStart();
        while ((IsOverlapping(a, d) || IsOverlapping(b, d) || IsOverlapping(c, d)) && Helper.NoInfiniteLoop)
        {
            x4 = Random.Range(topLeft.position.x + 1f, topRight.position.x - 1f);
            y4 = Random.Range(bottomRight.position.y + 1f, topRight.position.y - 1f);
            d = new Vector3(x4, y4, Helper.ZValueOfObstacle);
            center[3] = d;
        }
        //Debug.Log(center[0]);
        return center;
    }
    bool IsOverlapping(Vector3 a, Vector3 b)
    {
        bool myBool = true;
        if (Mathf.Abs(a.x - b.x) >= 4f || Mathf.Abs(a.y - b.y) >= 4f)
        {
            myBool = false;
        }
        return myBool;
    }

    void runObstacles()
    {
        int numOfObstacles = Random.Range(3, 5);
        Vector3[] a;
        if (numOfObstacles == 3)
        {
            a = pickCenter3();
        }
        else
        {
            a = pickCenter4();
        }
        foreach (var pos in a)
        {
            // create the obstacle and add its vertices to shapes
            Obstacle obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);
            //obstacles.Add(obstacle);
            //this is used for the reduced v map
            shapes.Add(obstacle.vertex);
            editorVertices.AddRange(obstacle.vertex);
        }

    }
    void buildRVG()
    {
        // construct our graph
        rvg = new RVG(editorVertices);
        // show the vertices
        foreach (var node in rvg.vertices)
        {
            Instantiate(pointPrefab, node.Position, Quaternion.identity);
        }

        // show the edges
        foreach (var (a, b) in rvg.edges)
        {
            Line line = Instantiate(linePrefab);
            line.SetVertices(a.Position, b.Position);
        }

        
    }
}