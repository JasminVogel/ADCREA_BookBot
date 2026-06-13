using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.TerrainTools;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using System.IO;

public class AStarPathfinder : MonoBehaviour
{
    [SerializeField] float waitingTime = 0.25f;
    public FloorGrid floorGrid;
   
    List<GridNode> openList = new List<GridNode>();
    List<GridNode> closedList = new List<GridNode>();


   
    public RobotManager managerOfRobot;


    public IEnumerator FindPath (Vector3 startPos, Vector3 targetPos)
    {
     
        GridNode startNode = floorGrid.NodeFromWorldPoint(startPos); 
        GridNode targetNode =   floorGrid.NodeFromWorldPoint(targetPos); 

        if(startNode == null || targetNode == null)
        {
            Debug.Log("Cannot caluclate path");
            yield break;
        }
        //making sure nothing previous is in there
        openList.Clear();
        closedList.Clear();

        openList.Add(startNode);

        while(openList.Count > 0)
        {
            GridNode currentNode = openList[0];

            for(int n = 0; n < openList.Count; n++)
            {
                if(openList[n].finalCost <  currentNode.finalCost)
                {
                    currentNode = openList[n];
                }
                
                

            }
            closedList.Add(currentNode);
            floorGrid.checkedNodes = closedList;
            openList.Remove(currentNode);

            if(currentNode == targetNode)
            {   
                RetracePath(startNode, targetNode);
                yield break;
            }

            foreach(GridNode neighbor in floorGrid.NeighborNodes(currentNode))
            {
                if(!neighbor.isWalkable || closedList.Contains(neighbor) )
                {
                    continue;
                }
                
                
                int newCostToNeighbor = currentNode.generalCost + GetDistance(currentNode, neighbor);


                if(newCostToNeighbor < neighbor.generalCost || !openList.Contains(neighbor))
                {
                    neighbor.generalCost = newCostToNeighbor;
                    if (SimulationSettings.useDijkstra)
                    {  
                        neighbor.heuristicCost = 0; 
                    }
                    else
                    {
                        neighbor.heuristicCost = GetDistance(neighbor, targetNode);
                    } 
                    neighbor.parentNode = currentNode;
                    
                    //no final cost analysis needed cuz already done in the GridNode.cs



                    if(!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
        
                
                
            }
            yield return new WaitForSeconds(waitingTime);

        }
        
        

    }


    public int GetDistance(GridNode nodeA, GridNode nodeB)
    {
       int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
       int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return (distanceX + distanceY)* 10;
    }


    void RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> pathToStart = new List<GridNode>();
        GridNode currentNode = endNode;  //Starting at the finishline and tracking backwards

        while(currentNode != startNode)
        {
            pathToStart.Add(currentNode);
            currentNode = currentNode.parentNode;

        }
        pathToStart.Reverse();
        floorGrid.finalPath = pathToStart;
        managerOfRobot.BookMustBeDelivered(pathToStart);

    }
}
