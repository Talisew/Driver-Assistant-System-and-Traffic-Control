using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding
{
    public static Vertex tmpVertex1;
    public static Vertex tmpVertex2;
    //Define a new class called node for eaiser calculation for the pathfinding
    public class Node
    {
        public readonly Vertex v;
        public readonly List<(Node Node, float Cost)> neighbors;
        public Node(Vertex v, int count)
        {
            this.v = v;
            neighbors = count > 0 ? new List<(Node Node, float Cost)>(count) : new List<(Node Node, float Cost)>();
        }

        public override int GetHashCode()
        {
            return v.Position.GetHashCode();
        }
    }
    private Node start;
    private Node dest;
    private RVG graph;

    // to do the pathfiding, we need the cost function
    //f(n)=g(n)+h(n)
    // the real distance of this vertex to the dest = h(n)
    public float hFunc(Node n)
    {
        return (n.v.Position - dest.v.Position).magnitude;
    }
    // be aware of !!!! vStart and vDest should also be ZValueOfObstacle.
    public PathFinding(RVG rvg, Vector3 vStart, Vector3 vDest)
    {
        // store the vertrices that with g(n) result in them...?
        List<Node> nodes = new List<Node>(rvg.vertices.Count + 2);

        tmpVertex1.Position = vStart;
        tmpVertex2.Position = vDest;

        start = new Node(tmpVertex1, 0);
        dest = new Node(tmpVertex2, 0);

        foreach (var vertex in rvg.vertices)
        {
            var node = new Node(vertex, vertex.neighbors.Count + 1);
            //temporary store it
            vertex.temporary = node;
            nodes.Add(node);
        }

        Vector3 pStart = new Vector3(vStart.x, vStart.y, Helper.ZValueOfObstacle);
        Vector3 pDest = new Vector3(vDest.x, vDest.y, Helper.ZValueOfObstacle);
        Vector3 tempo = new Vector3();
        // detect whether we can find a path or not.
        foreach (var node in nodes)
        {
            tempo.x = node.v.Position.x;
            tempo.y = node.v.Position.y;
            tempo.z = Helper.ZValueOfObstacle;
            // Does it have a start in its plan?
            if (!Physics.Linecast(pStart, tempo))
            {
                var cost = (vStart - node.v.Position).magnitude;
                start.neighbors.Add((node, cost));
                node.neighbors.Add((start, cost));
            }
            // Does it have the dest in its plan?
            if (!Physics.Linecast(pDest, tempo))
            {
                var cost = (vDest - node.v.Position).magnitude;
                dest.neighbors.Add((node, cost));
                node.neighbors.Add((dest, cost));
            }

            foreach (var (vertex, cost) in node.v.neighbors)
            {
                node.neighbors.Add((vertex.temporary, cost));
            }
        }
    }
    // learn the following method from Wiki
    public List<Vector3> constructA()
    {
        
        var openSet = new HashSet<Node>();
        openSet.Add(start);
        var cameFrom = new Dictionary<Node, Node>();
        var gScore = new Dictionary<Node, float>();
        gScore.Add(start, 0);

        var fn = new Dictionary<Node, float>();
        fn.Add(start, hFunc(start));

        while (openSet.Count != 0)
        {
            var cur = lowestCost(openSet, fn);
            if (cur == dest)
            {
                return ReconstructPath(cameFrom, cur);
            }

            openSet.Remove(cur);
            // cost func: f(n)=g(n)+h(n)
            foreach (var (neighbor, cost) in cur.neighbors)
            {
                var assumeGn = gScore.GetValueOrDefault(cur, float.PositiveInfinity) + cost;
                if (assumeGn < gScore.GetValueOrDefault(neighbor, float.PositiveInfinity))
                {
                    cameFrom[neighbor] = cur;
                    gScore[neighbor] = assumeGn;
                    fn[neighbor] = assumeGn + hFunc(neighbor);
                    // since we need to connect them.
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return new List<Vector3>();
    }
    //though we dont have to find the shortest path, we still follow the rule to choose the lowest cost plan.
    //so choose the lowest one.
    public Node lowestCost(HashSet<Node> set, Dictionary<Node, float> fn)
    {
        if (set != null)
        {
            Node lowest = null;
            // set the initial cost to inf
            float upperBoundCost = float.PositiveInfinity;
            foreach (var node in set)
            {
                float score = fn.GetValueOrDefault(node, float.PositiveInfinity);
                if (lowest == null || score < upperBoundCost)
                {
                    //continue until we find the lowest one
                    lowest = node;
                    upperBoundCost = score;
                }
            }
            return lowest;
        }
        else
        {
            return null;
        }
        
        
    }
    // we need to change the plan during the travel since we need to rearrange the plan when we arrive a new vertex
    public List<Vector3> ReconstructPath(Dictionary<Node, Node> cameFrom, Node cur)
    {
        Vector3 changedZ = new Vector3(cur.v.Position.x, cur.v.Position.y, Helper.ZValueOfAgent);
        List<Vector3> theNewPath = new List<Vector3>();
        theNewPath.Add(changedZ);
        while (cameFrom.ContainsKey(cur))
        {
            cur = cameFrom[cur];
            changedZ = new Vector3(cur.v.Position.x, cur.v.Position.y, Helper.ZValueOfAgent);
            theNewPath.Add(changedZ);
        }
        return theNewPath;
    }
    // return the path
    public static List<Vector3> FindPath(RVG theRVG, Vector3 start, Vector3 theDest)
    {
        Helper.NumOfPathPlanned();
        List<Vector3> thePath;
        // use this to count time.
        var sw = Stopwatch.StartNew();
        var dir = theDest - start;
        var pos = new Vector3(start.x, start.y, Helper.ZValueOfObstacle);
        bool canReachDirectly = !Physics.BoxCast(pos, Agent.shape, dir, Quaternion.identity, dir.magnitude);

        if (canReachDirectly)
        {
            //since pos.z=ZValueOfObstacle, now change it into ZValueOfAgent
            pos.z = Helper.ZValueOfAgent;
            thePath = new List<Vector3>() { theDest, pos };
        }
        else
        {
            var pathfinding = new PathFinding(theRVG, start, theDest);
            UnityEngine.Assertions.Assert.IsTrue(theDest != null, "dest is null");
            UnityEngine.Assertions.Assert.IsTrue(theRVG != null, "rvg is null");
            UnityEngine.Assertions.Assert.IsTrue(start != null, "agent position is null");

            thePath = pathfinding.constructA();
        }
        //stop 
        sw.Stop();
        Helper.TotalPlanningTime(sw.Elapsed);
        return thePath;
    }

   
}
