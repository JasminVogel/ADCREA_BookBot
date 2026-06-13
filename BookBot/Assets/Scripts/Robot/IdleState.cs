using UnityEngine;

public class IdleState : BaseState
{

    private float scanTimer = 0f;
    private float scanInterval = 0.1f;
    private bool isWaitingForBoss = false;

    public IdleState(RobotController _robot)
    {
        this.robot = _robot;
    }
    public override void Enter()
    {
        Debug.Log(" Robot is resting");
        isWaitingForBoss = false;
    }
    public override void Update()
    {
        if (isWaitingForBoss) return;
        scanTimer += Time.deltaTime;

        if (scanTimer >= scanInterval)
        {
            scanTimer = 0f;
            GridNode targetNode = robot.FindClosestBookBFS();
            
            if (targetNode != null)
            {
                Debug.Log($"BEEP!");
                isWaitingForBoss = true;
                robot.aStar.managerOfRobot.RequestPathToPile(targetNode.worldposition);
            }
        }    
    }
    public override void Exit()
    {
        
    }
}
