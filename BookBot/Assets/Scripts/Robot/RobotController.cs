using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class RobotController : MonoBehaviour
{
   public List<GridNode> currentPath = new List<GridNode>(); 
   public AStarPathfinder aStar;
   public float speed = 3f;
   int nextTargetIndex = 0;

    

    void Update()
    {
        
        
        if(currentPath != null && nextTargetIndex < currentPath.Count)
        {
            GridNode nextTarget = currentPath[nextTargetIndex];
            MoveToTarget(nextTarget);
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

    public void SetPath(List<GridNode> path)
    {
        currentPath = path;
        nextTargetIndex = 0;
        
    }

}
