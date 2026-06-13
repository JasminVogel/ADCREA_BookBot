using UnityEngine;

public class DeliveringState : BaseState
{
    private float deliveryTimer = 0f;
    private float timeToDeliver = 2.0f;

    public DeliveringState(RobotController _robot)
    {
        this.robot = _robot;
    }


    public override void Enter()
    {
        Debug.Log("Robot: Reached the shelf. Starting delivery protocol...");
        deliveryTimer = 0f; 
    }


    public override void Update()
    {
        
        deliveryTimer += Time.deltaTime;

      
        if (deliveryTimer >= timeToDeliver)
        {
            Debug.Log("Robot: Book delivered! Going back to Idle.");
            robot.SwitchState(new IdleState(robot));
        }
    }

    public override void Exit()
    {
        // Clean up here
    }

}
