using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VectorGraphics;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    //if size of ground would be scaled, change here
    [SerializeField] private int gridSizeX = 25;
    [SerializeField] private int gridSizeY = 15;
    GridNode[,] gridNodes;


    public LayerMask obstacleMask;
    public List<GridNode> finalPath;
    public List<GridNode> checkedNodes;
    void Awake()
    {
        CreateGrid();
    }

    public GridNode NodeFromWorldPoint(Vector3 positionInWorld)
    {

        //rounding for Arrays (because of mouse clicks)
        int x = Mathf.RoundToInt(positionInWorld.x);
        int y = Mathf.RoundToInt(positionInWorld.z);  


        
        if (gridNodes != null && x >= 0 && x < gridNodes.GetLength(0) && y >= 0 && y < gridNodes.GetLength(1))
        {
            return gridNodes[x,y];
        }
        else
        {
            return null;
        }
    }

    public List<GridNode> NeighborNodes(GridNode node)
    {
        
        List<GridNode> neighbor = new List<GridNode>();
        if (gridNodes == null) 
        { 
            return neighbor;
        }

        for(int x = -1; x <= 1; x ++)
        {
            for( int y = -1; y <= 1; y ++)
            {
                if( x == 0 && y == 0 )
                {
                    continue;
                }    

               
                if(Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                {
                
                    continue;   
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                
                if(0<= checkX && checkX <gridSizeX)
                {
                    if(0<= checkY && checkY <gridSizeY)
                    {
                        neighbor.Add(gridNodes[checkX,checkY]);
                    }
                    
                }

               

            }
        }

        return neighbor;


    }

    void OnDrawGizmos()
    {
        if(gridNodes != null)
        {
            foreach(GridNode node in gridNodes)
            {
                if(node.isWalkable == true)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
               

                if(checkedNodes != null && checkedNodes.Contains(node))
                {
                    Gizmos.color = Color.cornflowerBlue;
                }

                if(finalPath !=null && finalPath.Contains(node))
                {
                    Gizmos.color = Color.black;

                }
                Gizmos.DrawCube(node.worldPosition, new Vector3(0.95f, 0.1f, 0.95f));
            }
        }
    }

    void CreateGrid()
    {
        gridNodes = new GridNode[gridSizeX, gridSizeY];
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y =0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = new Vector3(x,0,y);

                bool hitObstacle = Physics.CheckSphere(worldPoint, 0.4f, obstacleMask);
                bool isWalkable;
                if(hitObstacle == true)
                {
                    isWalkable = false;
                }
                else
                {
                    isWalkable = true;
                }


                gridNodes[x,y] = new GridNode(isWalkable,worldPoint,x,y);
            }
        }

    }

}
