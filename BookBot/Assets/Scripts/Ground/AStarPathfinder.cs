using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AStarPathfinder : MonoBehaviour
{
    //technically dijkstra as finalCost calculation is not in here
    
    [SerializeField] private float waitingTime = 0.25f;
    public FloorGrid floorGrid;
    public LineRenderer pathLine;
    public RobotManager managerOfRobot;
    public GameObject searchTilePrefab;


    private List<GameObject> activeSearchTiles = new List<GameObject>();
    List<GridNode> openList = new List<GridNode>();
    List<GridNode> closedList = new List<GridNode>();


   
  

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


        //for visualisation
        if (pathLine != null)
        {
            pathLine.positionCount = 0; 
        }

        ClearAllSearchTiles();
        




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

            //for visualisation
            if (searchTilePrefab != null)
            {
          
            GameObject newTile = Instantiate(searchTilePrefab, currentNode.worldPosition + (Vector3.up * 0.05f), Quaternion.Euler(90f, 0f, 0f));
            activeSearchTiles.Add(newTile);
            }




            floorGrid.checkedNodes = closedList;
            openList.Remove(currentNode);

            if(currentNode == targetNode)
            {   
                ClearAllSearchTiles();
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

    private void ClearAllSearchTiles()
    {
      foreach (GameObject tile in activeSearchTiles)
        {
            Destroy(tile);
        }
        activeSearchTiles.Clear();  
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

        // Visualisation for game window
        if (pathLine != null)
        {
            pathLine.positionCount = pathToStart.Count;
            for (int i = 0; i < pathToStart.Count; i++)
            {
                
                pathLine.SetPosition(i, pathToStart[i].worldPosition + (Vector3.up * 0.2f));
            }
        }
        managerOfRobot.BookMustBeDelivered(pathToStart);

    }
}
