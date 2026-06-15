using UnityEngine;

public class GridNode 
{

    //public because of A* 
    public GridNode parentNode;
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public bool isWalkable;

    //A* stuff
    public int generalCost;
    public int heuristicCost;

    public int finalCost
    {

        // remove heuristicCost then it is djistra
        get{ return generalCost + heuristicCost;}
    }
    

   //Infos for later visualization and positions 
    public GridNode(bool isWalkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
