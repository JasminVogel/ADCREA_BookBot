using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    //if size of ground would be scaled, change here
    GridNode[,] gridNodes = new GridNode[15, 25];
    public LayerMask obstacleMask;
    void Awake()
    {
        for(int x = 0; x < 15; x++)
        {
            for(int y =0; y < 25; y++)
            {
                Vector3 worldPoint = new Vector3(x,0,y);
                bool walkable = !Physics.CheckSphere(worldPoint, 0.4f, obstacleMask);


                gridNodes[x,y] = new GridNode(walkable,worldPoint,x,y);
            }
        }
    }

    public GridNode NodeFromWorldPoint(Vector3 positionInWorld)
    {
        int x = Mathf.RoundToInt(positionInWorld.x);
        int y = Mathf.RoundToInt(positionInWorld.z);  //IMPORTANT Y&Z switch


        Debug.Log("Tchecking if node is on grid");
        if(x >= 0 && x<15 && y>= 0 && y<25)
        {
            return gridNodes[x,y];
        }

        Debug.Log("This Node is outside of the map");
        return null;
    }

    void OnDrawGizmos()
    {
        if(gridNodes != null)
        {
            foreach(GridNode node in gridNodes)
            {
                Gizmos.color = node.isWalkable ? Color.white : Color.red;

                Gizmos.DrawCube(node.worldposition, new Vector3(0.9f, 0.1f, 0.9f));
            }
        }
    }
}
