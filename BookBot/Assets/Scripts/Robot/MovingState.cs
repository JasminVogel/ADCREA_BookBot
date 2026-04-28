using System.Collections.Generic;
using UnityEngine;

public class MovingState :  BaseState
{
    public MovingState(RobotController _robot, List<GridNode> path)
    {
        this.robot = _robot;
        robot.currentPath = path;
        robot.nextTargetIndex = 0;
        
    }
    public override void Enter()
    {
        
    }
    public override void Update()
    {
       if(robot.currentPath != null && robot.nextTargetIndex < robot.currentPath.Count)
        {
            GridNode nextTarget = robot.currentPath[robot.nextTargetIndex];
            MoveToTarget(nextTarget);
        }
        else
        {
            robot.SwitchState(new IdleState(robot));
        }
    }
    public override void Exit()
    {
        
    }

     public void MoveToTarget(GridNode nextTarget)
    {
        
        robot.transform.position = Vector3.MoveTowards(robot.transform.position, nextTarget.worldposition, robot.speed * Time.deltaTime);
        float distanceToTarget = Vector3.Distance(robot.transform.position, nextTarget.worldposition);
        if(distanceToTarget < 0.1f )
        {
          robot.nextTargetIndex ++; 
        }
    }

}
