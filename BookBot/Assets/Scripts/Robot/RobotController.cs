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
   public int currentTargetSlot = 0;

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

    public GridNode FindClosestBookBFS()
    {
       
        GridNode startNode = aStar.floorGrid.NodeFromWorldPoint(transform.position);
        if (startNode == null) return null;

        Queue<GridNode> queue = new Queue<GridNode>();
        HashSet<GridNode> visited = new HashSet<GridNode>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

     
        while (queue.Count > 0)
        {
            GridNode currentNode = queue.Dequeue();

      
            Collider[] hits = Physics.OverlapSphere(currentNode.worldposition + Vector3.up * 1f, 1.5f);
            foreach (Collider hit in hits)
            {
                Book foundBook = hit.GetComponent<Book>();
                if (foundBook != null && !foundBook.isSorted)
                {
                    return currentNode; 
                }
            }

            
            foreach (GridNode neighbor in aStar.floorGrid.NeighborNodes(currentNode))
            {
                if (!visited.Contains(neighbor) && neighbor.isWalkable)
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
            
        }
        
        return null; 
    }

    public int ScanForCorrectSlot(Shelf targetShelf, Color bookColor)
    {
        int bestSlot = 0;
        float smallestDifference = float.MaxValue;

        Color.RGBToHSV(bookColor, out float targetH, out float targetS, out float targetV);

        for (int i = 0; i < targetShelf.booksPerRow; i++)
        {
            float percentage = (float)i / (targetShelf.booksPerRow - 1);
            
            Color idealSlotColor = Color.Lerp(targetShelf.lightColor, targetShelf.darkColor, percentage);
            Color.RGBToHSV(idealSlotColor, out float idealH, out float idealS, out float idealV);

            float difference = Mathf.Abs(targetH - idealH) + Mathf.Abs(targetS - idealS) + Mathf.Abs(targetV - idealV);

            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                bestSlot = i;
            }
        }

        
        int flippedSlot = (targetShelf.booksPerRow - 1) - bestSlot;
        
        Debug.Log($"Algorithm calculated Slot {bestSlot}. Flipping to correct physical Slot {flippedSlot}!");

        return flippedSlot;
    }
}
