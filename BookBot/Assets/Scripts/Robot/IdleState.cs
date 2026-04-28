using UnityEngine;

public class IdleState : BaseState
{

        public IdleState(RobotController _robot)
    {
        this.robot = _robot;
    }
    public override void Enter()
    {
        Debug.Log(" Robot is resting");
    }
    public override void Update()
    {
        
    }
    public override void Exit()
    {
        
    }
}
