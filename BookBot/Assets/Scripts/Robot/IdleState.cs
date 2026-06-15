using UnityEngine;

public class IdleState : BaseState
{

    private float scanTimer = 0f;
    private float scanInterval = 0.1f;
    private bool isWaitingForManager = false;

    public IdleState(RobotController robot)
    {
        this.robot = robot;
    }
    public override void Enter()
    {
        Debug.Log(" Robot is resting");
        isWaitingForManager = false;
    }
    public override void Update()
    {

        //contantly checks if there are more books to sort
        if (isWaitingForManager) return;
        scanTimer += Time.deltaTime;

        if (scanTimer >= scanInterval)
        {
          ScanForBooks();
        }    
    }

    private void ScanForBooks()
    {
        scanTimer = 0f;
        GridNode targetNode = robot.FindClosestBookBFS();
            
        if (targetNode != null)
        {
            isWaitingForManager = true;
            robot.aStar.managerOfRobot.RequestPathToPile(targetNode.worldPosition);
        }
    }
    public override void Exit()
    {
        
    }
}
