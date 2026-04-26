using UnityEngine;

public class GridNode 
{

    public Vector3 worldposition;

    public int gridX;
    public int gridY;

    public bool isWalkable;

    //A* stuff
    public int generalCost;
    public int heuristicCost;

    public int finalCost
    {
        get{ return generalCost + heuristicCost;}
    }
    GridNode parentNode;


    // _ stands for temporaty assignment
    public GridNode(bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        isWalkable = _isWalkable;
        worldposition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
}
