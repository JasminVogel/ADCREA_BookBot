using System.Collections.Generic;
using System.IO;
using NUnit.Framework.Internal;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public RobotController robot;
    public AStarPathfinder pathfinder;
    public Transform testTarget;



    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Boss: Caluclating route to target...");
            pathfinder.StartCoroutine(pathfinder.FindPath(robot.transform.position, testTarget.position));
        }
    }
    public void BookMustBeDelivered(List<GridNode> path)
    {
        robot.SwitchState(new MovingState(robot, path));

    }

}
