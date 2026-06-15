using System.Collections.Generic;
using UnityEngine;

public class MovingState :  BaseState
{

    private bool isRecalculating = false;
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
        if (isRecalculating) return; 

        if (robot.currentPath != null && robot.nextTargetIndex < robot.currentPath.Count)
        {
            GridNode nextTarget = robot.currentPath[robot.nextTargetIndex];
            Vector3 currentPos = robot.transform.position;
            Vector3 targetPos = nextTarget.worldposition;
            currentPos.y = 0;
            targetPos.y = 0;
            Vector3 moveDirection = (nextTarget.worldposition - robot.transform.position).normalized;
            

            int obstacleMask = 1 << LayerMask.NameToLayer("Obstacle");
       
          if (Physics.Raycast(robot.transform.position + (Vector3.up * 0.5f), moveDirection, out RaycastHit hit, 1.5f, obstacleMask))
            {
                Debug.LogWarning("LiDAR triggered! Wet floor sign detected. Halting for recalculation!");
                
                isRecalculating = true;

                GridNode blockedNode = robot.aStar.floorGrid.NodeFromWorldPoint(hit.point);
                if (blockedNode != null)
                {
                    blockedNode.isWalkable = false;
                }

                Book heldBook = robot.GetComponentInChildren<Book>();
                if (heldBook == null)
                {
                    Vector3 pileDestination = robot.currentPath[robot.currentPath.Count - 1].worldposition;
                    robot.aStar.managerOfRobot.RequestPathToPile(pileDestination);
                }
                else
                {
                    robot.aStar.managerOfRobot.RequestPathToShelf(heldBook.myShelf);
                }
                
                return; 
            }
            
            MoveToTarget(nextTarget);
        }
        else
        {
            Book heldBook = robot.GetComponentInChildren<Book>();
            
            if (heldBook == null)
            {
                robot.SwitchState(new PickingState(robot));
            }
            else
            {
                robot.SwitchState(new DroppingState(robot)); 
            }
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
