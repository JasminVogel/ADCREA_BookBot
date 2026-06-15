using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VectorGraphics;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    //if size of ground would be scaled, change here
    public int gridSizeX = 25;
    public int gridSizeY = 15;
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
        int x = Mathf.RoundToInt(positionInWorld.x);
        int y = Mathf.RoundToInt(positionInWorld.z);  //IMPORTANT Y&Z switch


        Debug.Log("checking if node is on grid");
        if (gridNodes != null && x >= 0 && x < gridNodes.GetLength(0) && y >= 0 && y < gridNodes.GetLength(1))
        {
            return gridNodes[x,y];
        }
        else
        {
        Debug.Log($"This Node is outside of the map at World Position: {positionInWorld}");
        return null;
        }
    }

    public List<GridNode> NeighborNodes(GridNode node)
    {
        //new list for finding connected nodes (since not really a tree, this is how we do it, 3x3 field)
        List<GridNode> neighbor = new List<GridNode>();
        if (gridNodes == null) 
        { 
            return neighbor;
        }
        int actualGridWidth = gridNodes.GetLength(0);
        int actualGridHeight = gridNodes.GetLength(1);

        for(int x = -1; x <= 1; x ++)
        {
            for( int y = -1; y <= 1; y ++)
            {
                if( x == 0 && y == 0 )
                {
                    continue;
                }    

                 //Because of movement restrictions (diagonally)
                if(Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                {
                
                    continue;   
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //for the walls, that there is no error outside walls
                if(0<= checkX && checkX <gridSizeX && 0<= checkY && checkY <gridSizeY)
                {

                    neighbor.Add(gridNodes[checkX,checkY]);
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
                Gizmos.color = node.isWalkable ? Color.white : Color.red;
                if(checkedNodes != null && checkedNodes.Contains(node))
                {
                    Gizmos.color = Color.cornflowerBlue;
                }

                if(finalPath !=null && finalPath.Contains(node))
                {
                    Gizmos.color = Color.black;

                }
                Gizmos.DrawCube(node.worldposition, new Vector3(0.95f, 0.1f, 0.95f));
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


                bool walkable = !Physics.CheckSphere(worldPoint, 0.4f, obstacleMask);


                gridNodes[x,y] = new GridNode(walkable,worldPoint,x,y);
            }
        }

    }

}
