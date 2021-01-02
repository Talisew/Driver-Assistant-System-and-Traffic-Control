using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer LineRenderer
    {
        get { return GetComponent<LineRenderer>(); }
    }


    /*public Color Color
    {
        get { return LineRenderer.startColor; }
        set
        {
            LineRenderer.startColor = value;
            LineRenderer.endColor = value;
        }
    }*/

    public Vector3[] points
    {
        get
        {
            Vector3[] positions = new Vector3[LineRenderer.positionCount];
            LineRenderer.GetPositions(positions);
            return positions;
        }
        set
        {
            LineRenderer.positionCount = value.Length;
            LineRenderer.SetPositions(value);
        }
    }
    public void SetVertices(Vector3 start, Vector3 end)
    {
        points = new Vector3[] { start, end };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
