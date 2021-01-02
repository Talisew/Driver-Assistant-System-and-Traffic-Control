using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    // main area to choose
    // range of the whole basic level(not just the center one)
    // not the true range, like true range -1, since the agent has a non-0 area
    public static float yMin = -6;
    public static float yMax = 6;
    public static float xMin = -7;
    public static float xMax = 7;
    private static readonly int maxIter = 8000;
    private static int iter = 0;
    
    public static float ZValueOfObstacle = -2;
    public static float ZValueOfVertex = -4;
    public static float ZValueOfAgent = -6;


    public static int numOfSuccessReach = 0;
    public static int numOfPath = 0;
    public static System.TimeSpan totalPlanningTime = System.TimeSpan.Zero;
    public static int numOfReplan = 0;
    public static void NumOfSuccess()
    {
        numOfSuccessReach++;
    }
    public static void NumOfPathPlanned()
    {
        numOfPath++;
    }

    public static void TotalPlanningTime(System.TimeSpan time)
    {
        totalPlanningTime += time;
    }
    public static void NumOfPathReplan()
    {
        numOfReplan++;
    }
    
    public static void NoInfiniteLoopStart()
    {
        iter = 0;
    }
    public static bool NoInfiniteLoop
    {
        get
        {
            bool value = iter++ < maxIter;
            if (!value)
            {
                Debug.Log("Infinite loop breaked");
            }
            return iter++ < maxIter;
        }
    }

    // generate a random position for the agent
    public static Vector3 chooseRandomPos(Vector3 shapes)
    {
        // detect whether the agent position is allowed
        while (true)
        {
            var position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 1);
            //do the raycast for four times since the agent is a non-0 area
            bool onTheLevel = true;
            float radius = shapes.x;
            Vector3 agentPosition = new Vector3(position.x + radius,position.y+radius,position.z);
            if (!Physics.Raycast(agentPosition, Vector3.back, 2))
            {
                onTheLevel= false;
            }
            agentPosition.y = agentPosition.y - 2 * radius;
            if (!Physics.Raycast(agentPosition, Vector3.back, 2))
            {
                onTheLevel= false;
            }
            agentPosition.x = agentPosition.x - 2 * radius;
            if (!Physics.Raycast(agentPosition, Vector3.back, 2))
            {
                onTheLevel=false;
            }
            agentPosition.y = agentPosition.y + 2 * radius;
            if (!Physics.Raycast(agentPosition, Vector3.back, 2))
            {
                onTheLevel = false;
            }
            // StrictCubeCast(position, shapes.x * 2); // if it hit something, it must be the floor
            //regenerate another position since it's not on the basic level
            if (onTheLevel)
            {
                position.z = 0;
                // test whether there's obstacle on the position
                bool isOverlap = Physics.BoxCast(position, shapes, Vector3.back);
                //regenerate another position if it really has
                if (isOverlap)
                {
                    continue;
                }
                return position;
            }
            else
            {
                continue;
            }
            
        }
    }

    //make a dictionary
    public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
    {
        return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
    }
}
