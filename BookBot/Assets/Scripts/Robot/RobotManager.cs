using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public RobotController robot;
    public AStarPathfinder pathfinder;
    public Shelf[] allShelves;
  
    public Dictionary<Shelf, Vector3> shelfDeliveryZone = new Dictionary<Shelf, Vector3>();


    void Start()
    {
        //Scan shelfs
        foreach (Shelf shelf in allShelves)
        {
            if (shelf != null)
            {
                shelfDeliveryZone.Add(shelf, shelf.GetDeliveryZone());
            }
        }
        Debug.Log("found location of deliveryzone ");
    }

    public void RequestPathToPile(Vector3 pilePosition)
    {
        Debug.Log(" BFS Scanner saw books. calculating path");
        StartCoroutine(pathfinder.FindPath(robot.transform.position, pilePosition));
    }

    public void RequestPathToShelf(Shelf targetShelf)
    {
        
        if (shelfDeliveryZone.TryGetValue(targetShelf, out Vector3 zonePos))
        {
            Debug.Log(" calculating route to next shelf");
            StartCoroutine(pathfinder.FindPath(robot.transform.position, zonePos));
        }
    }

   
    public void BookMustBeDelivered(List<GridNode> path)
    {
        robot.SwitchState(new MovingState(robot, path));

    }

}
