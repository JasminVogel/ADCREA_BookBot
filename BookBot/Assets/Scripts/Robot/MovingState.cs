using System.Collections.Generic;
using UnityEngine;

public class MovingState : BaseState
{
    private bool isRecalculating = false;

    // We replace the "Magic Numbers" with clearly named variables at the top
    private float sensorHeight = 0.5f;
    private float sensorRange = 1.2f;

    public MovingState(RobotController robot, List<GridNode> path)
    {
        this.robot = robot;
        this.robot.currentPath = path;
        this.robot.nextTargetIndex = 0;
    }

    public override void Enter()
    {
    }


    public override void Update()
    {
        if (isRecalculating) 
        {
            return;
        } 

        if (HasReachedDestination())
        {
            TransitionToNextTask();
        }
        else
        {
            FollowPath();
        }
    }

    public override void Exit()
    {
    }



    private bool HasReachedDestination()
    {
        //make sure robot stops
        return robot.currentPath == null || robot.nextTargetIndex >= robot.currentPath.Count;
    }

    private void FollowPath()
    {
        GridNode nextTarget = robot.currentPath[robot.nextTargetIndex];
        
        Vector3 currentPos = robot.transform.position;
        Vector3 targetPos = nextTarget.worldPosition;

        //saftey against floating
        currentPos.y = 0;
        targetPos.y = 0;
        
        
        Vector3 moveDirection = (targetPos - currentPos).normalized;

        if (IsPathBlocked(moveDirection))
        {
            RecalculatePath();
            return; 
        }
        
        MoveToTarget(nextTarget);
    }

    private bool IsPathBlocked(Vector3 moveDirection)
    {
        int obstacleMask = LayerMask.GetMask("Obstacle");
        RaycastHit hit;
        
        //elevate to hit floor signs
        Vector3 sensorOrigin = robot.transform.position + (Vector3.up * sensorHeight);

        if (Physics.Raycast(sensorOrigin, moveDirection, out hit, sensorRange, obstacleMask))
        {
            Debug.LogWarning("LiDAR triggered! Wet floor sign detected. Halting for recalculation!");
            
            
            GridNode blockedNode = robot.aStar.floorGrid.NodeFromWorldPoint(hit.point);
            if (blockedNode != null)
            {
                blockedNode.isWalkable = false;
            }
            
            return true;
        }
        
        return false;
    }

    private void RecalculatePath()
    {
        isRecalculating = true;

        Book heldBook = robot.GetComponentInChildren<Book>();
        
        //move to book pile or shelf
        if (heldBook == null)
        {
            Vector3 pileDestination = robot.currentPath[robot.currentPath.Count - 1].worldPosition;
            robot.aStar.managerOfRobot.RequestPathToPile(pileDestination);
        }
        else
        {
            robot.aStar.managerOfRobot.RequestPathToShelf(heldBook.myShelf);
        }
    }

    private void TransitionToNextTask()
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

    public void MoveToTarget(GridNode nextTarget)
    {
        robot.transform.position = Vector3.MoveTowards(robot.transform.position, nextTarget.worldPosition, robot.speed * Time.deltaTime);
        float distanceToTarget = Vector3.Distance(robot.transform.position, nextTarget.worldPosition);
        
        if (distanceToTarget < 0.1f)
        {
            robot.nextTargetIndex++; 
        }
    }
}