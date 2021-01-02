using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public bool isReflex = false;
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public Vertex prev;
    public Vertex next;
    
    // add nerighbors so we can be easier while doing pathfinding
    // store by vertrices &distance from this to the stored vertrices
    // this one including all the vertrices and distance of them this vertex can connect
    // assign value in the RVG. 
    public List<(Vertex Vertex, float Cost)> neighbors = new List<(Vertex, float)>();

    //temporary storage
    public PathFinding.Node temporary;
    // Start is called before the first frame update
    void Start()
    {
        var pos = transform.position;
        pos.z = Helper.ZValueOfVertex;
        transform.position = pos;

        if (!isReflex)
        {
            gameObject.SetActive(false);
        }
    }

    public override int GetHashCode()
    {
        return transform.position.GetHashCode();
    }
}
