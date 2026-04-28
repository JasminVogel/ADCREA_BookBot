using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class RobotController : MonoBehaviour
{
   public List<GridNode> currentPath = new List<GridNode>(); 
   public AStarPathfinder aStar;

   private BaseState activeState;
   public float speed = 3f;
   public int nextTargetIndex = 0;

    public void Start()
    {
        SwitchState(new IdleState(this));
    }
    public void SwitchState(BaseState newState)
    {
        if(activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;
        activeState.Enter();
    }
    void Update()
    {
        if(activeState != null)
        {
            activeState.Update();
        }
        
    }
    public void MoveToTarget(GridNode nextTarget)
    {
        
        transform.position = Vector3.MoveTowards(transform.position, nextTarget.worldposition, speed * Time.deltaTime);
        float distanceToTarget = Vector3.Distance(transform.position, nextTarget.worldposition);
        if(distanceToTarget < 0.1f )
        {
          nextTargetIndex ++; 
        }
    }
}
