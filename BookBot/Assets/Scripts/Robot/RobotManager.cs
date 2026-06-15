using System.Collections.Generic;
using System.IO;
using NUnit.Framework.Internal;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public RobotController robot;
    public AStarPathfinder pathfinder;
    public Shelf[] allShelves;
  
    public Dictionary<Shelf, Vector3> deliveryZoneCache = new Dictionary<Shelf, Vector3>();


    void Start()
    {

        foreach (Shelf shelf in allShelves)
        {
            if (shelf != null)
            {
                deliveryZoneCache.Add(shelf, shelf.GetDeliveryZone());
            }
        }
        Debug.Log($"Boss: Cached {deliveryZoneCache.Count} delivery zones in the Dictionary.");
    }

    public void RequestPathToPile(Vector3 pilePosition)
    {
        Debug.Log("Boss: BFS Scanner detected books! Calculating A* route to the pile...");
        StartCoroutine(pathfinder.FindPath(robot.transform.position, pilePosition));
    }

    public void RequestPathToShelf(Shelf targetShelf)
    {
        
        if (deliveryZoneCache.TryGetValue(targetShelf, out Vector3 zonePos))
        {
            Debug.Log($"Boss: Calculating A* route to the {targetShelf.name}...");
            StartCoroutine(pathfinder.FindPath(robot.transform.position, zonePos));
        }
    }

   
    public void BookMustBeDelivered(List<GridNode> path)
    {
        robot.SwitchState(new MovingState(robot, path));

    }

}
