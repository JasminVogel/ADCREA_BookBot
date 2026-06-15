using UnityEngine;

public abstract class BaseState 
{

    //blueprint
    protected RobotController robot;
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

}
