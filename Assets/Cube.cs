using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    public Vector3 TopLeft
    {
        get { return topLeft.transform.position; }
    }
    public Vector3 TopRight
    {
        get { return topRight.transform.position; }
    }
    public Vector3 BottomLeft
    {
        get { return bottomLeft.transform.position; }
    }
    public Vector3 BottomRight
    {
        get { return bottomRight.transform.position; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
