using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStatus { Moving, Reached, Awaiting }
public class Agent : MonoBehaviour
{
    //since we give each vertex at least 0.01
    //if too small, change
    public static readonly float radius = 0.09f;
    //form the shape
    public static readonly Vector3 shape = new Vector3(radius, radius, 1);
    public static RVG rvg;
    //use this for the color part
    public static int count = 0; 
    public List<Vector3> path;
    private AgentStatus status;
    public Vector3 dest;
    public GameObject showDest;
    public GameObject destPrefab;
    //set some color to choose
    private static Color[] colors = new Color[] { Color.green, Color.yellow, Color.blue, Color.grey, Color.red, Color.black };

    public SpriteRenderer Renderer { get { return GetComponent<SpriteRenderer>(); } }
    public int numOfFailAttempt = 0;
    public int numOfCollision = 0;


    // Start is called before the first frame update 
    void Start()
    {
        // if the RVG is null, then we nned to define it.
        if (rvg is null)
        {
            var level = FindObjectOfType<LevelManager>();
            rvg = level.rvg;
            UnityEngine.Assertions.Assert.IsTrue(level != null, "level initialization is null");
        }
        
        // I will use this to set color for each agent
        if (count < colors.Length)
        {
            Renderer.color = colors[count];
        }
        else
        {
            var color = Random.ColorHSV();
            color.a = 1;
            Renderer.color = color;
        }
        showDest = Instantiate(destPrefab);
        // new add
        var position = Helper.chooseRandomPos(shape);
        position.z = Helper.ZValueOfAgent;
        dest = position;
        showDest.transform.position = dest;
        showDest.GetComponent<Renderer>().material.color = Renderer.color;
        count++;
        moveAlongPath();   
    }
    void FixedUpdate()
    {
        switch (status)
        {
            case AgentStatus.Moving:
                if (numOfCollision == 0)
                {
                    move();
                }
                

                break;
            case AgentStatus.Reached:
                Invoke(nameof(moveAlongPath), Random.Range(0.2f, 1));
                status = AgentStatus.Awaiting;

                break;
            default:
                break;
        }

    }
    
    void move()
    {
        // set the speed
        float speed = 3.0f;
        //move along the time
        float step = speed * Time.deltaTime;
        if (path.Count > 0)
        {
            var target = path[path.Count - 1];
            // pick the vertex and move towards to it
            transform.position = Vector3.MoveTowards(transform.position, target, step);


            // move to the chosen place and cut it from the path plan.
            if (Vector3.Distance(transform.position, target) < 0.005f)
            {
                path.RemoveAt(path.Count - 1);
            }
        }
        else
        {
            status = AgentStatus.Reached;
            Helper.NumOfSuccess();
            status = AgentStatus.Awaiting;
            Invoke(nameof(moveToNewDest), Random.Range(0.2f, 1));
            return;
        }
        
        
        //Debug.Log("MoveSuccess");
        
    }
    public void generateDest()
    {
        dest = Helper.chooseRandomPos(Agent.shape);
        dest.z = Helper.ZValueOfAgent;
        
        showDest.transform.position = dest;
    }
    
    void moveAlongPath()
    {
        //generateDest();
        
        path = PathFinding.FindPath(rvg, transform.position, dest);
        UnityEngine.Assertions.Assert.IsTrue(dest != null, "dest is null");
        UnityEngine.Assertions.Assert.IsTrue(rvg != null, "rvg is null");
        UnityEngine.Assertions.Assert.IsTrue(transform.position != null, "agent position is null");
        
        status = AgentStatus.Moving;
        move();
    }

    void moveToNewDest()
    {
        var position = Helper.chooseRandomPos(shape);
        position.z = Helper.ZValueOfAgent;
        dest = position;
        showDest.transform.position = dest;

        moveAlongPath();
    }
        // Update is called once per frame
        void Update()
    {
        
    }

void tryAgain()
{
    Helper.NumOfPathReplan();
    // if we fail more than three times just change the dest.
    if (numOfFailAttempt >= 3)
    {
        numOfFailAttempt = 0;
        moveToNewDest();
    }
    else
    {
        numOfFailAttempt++;
        // try again
        moveAlongPath();
    }
}


    void moveBack()
    {
        float speed = 3.0f;
        if (path.Count == 0) return;
        float step = speed * Time.deltaTime * -0.5f;
        var target = path[path.Count - 1];

        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
    private void OnTriggerEnter(Collider other)
    { 
        numOfCollision++;
        moveBack();
    }
    private void OnTriggerExit(Collider other)
    {
        // mius the num of collision if we successfully avoid it;
        numOfCollision--;
    }
    private void OnTriggerStay(Collider other)
    {
        if (status != AgentStatus.Moving)
        {
            return;
        }

        status = AgentStatus.Awaiting;

        
        var thisToThat = other.transform.position - transform.position;
        var offset = (radius*2) - thisToThat.magnitude;
        thisToThat.Normalize();
        transform.position = transform.position + 1.2f * offset * thisToThat; 
        other.transform.position = other.transform.position - 1.2f * offset * thisToThat;

        Invoke(nameof(tryAgain), Random.Range(0.1f, 0.5f));
    }
}

