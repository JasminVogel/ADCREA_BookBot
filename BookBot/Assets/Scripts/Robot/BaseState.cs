using UnityEngine;

public abstract class BaseState 
{
   protected RobotController robot;
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

}
