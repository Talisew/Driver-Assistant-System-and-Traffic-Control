using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    //we need to control the number of the agents
    public int numOfAgents;
    public Agent agentPrefab;
    
    private List<Agent> agents = new List<Agent>();
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(generateAgent), 0.4f);
    }
    // Update is called once per frame
    void Update()
    {

    }
    void generateAgent()
    {
        while (agents.Count < numOfAgents)
        {
            var position = Helper.chooseRandomPos(Agent.shape);
            position.z = Helper.ZValueOfAgent;
            //generate the agent
            agents.Add(Instantiate(agentPrefab, position, Quaternion.identity));
        }

    }
    
}
