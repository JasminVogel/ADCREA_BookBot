using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObstaclePlacer : MonoBehaviour
{
   public GameObject wetFloorSignPrefab;
    public LayerMask floorLayer; 
    public FloorGrid grid; 
    public RobotManager robotManager;

    private Dictionary<GridNode, GameObject> activeSigns = new Dictionary<GridNode, GameObject>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    private void HandleMouseClick()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return; 

        if (grid == null || robotManager == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, floorLayer))
        {
            GridNode clickedNode = grid.NodeFromWorldPoint(hit.point);

            if (clickedNode != null)
            {
                if (!clickedNode.isWalkable)
                {
                    TryRemoveSignAtNode(clickedNode);
                }
                else
                {
                    TryPlaceSignAtNode(clickedNode);
                }
            }
        }
    }

    private void TryRemoveSignAtNode(GridNode node)
    {
        // Instead of asking Unity physics to find the sign, we just check our memory!
        if (activeSigns.ContainsKey(node))
        {
            // 1. Destroy the physical 3D sign
            Destroy(activeSigns[node]); 
            
            // 2. Remove it from our memory bank
            activeSigns.Remove(node);   
            
            // 3. Unlock the tile for the robot!
            node.isWalkable = true;     
            Debug.Log("Sign removed via Dictionary bypass! Tile is unlocked.");
        }
        else
        {
            // If the player clicks an unwalkable tile (like a permanent shelf), do nothing!
            Debug.Log("Clicked an unwalkable tile, but no Wet Floor Sign is registered here.");
        }
    }

    private void TryPlaceSignAtNode(GridNode node)
    {
        if (IsForbiddenZone(node)) return; 

        Vector3 spawnPos = node.worldposition + (Vector3.up * 0.5f);

        // Spawn the sign and save a reference to it
        GameObject newSign = Instantiate(wetFloorSignPrefab, spawnPos, Quaternion.identity);
        
        // --- ADD TO MEMORY ---
        // Lock it into our Dictionary so we can find it instantly later
        activeSigns.Add(node, newSign);
        
        node.isWalkable = false;
        Debug.Log("Sign placed and registered in Dictionary!");
    }

    private bool IsForbiddenZone(GridNode node)
    {
        foreach (Vector3 zonePos in robotManager.deliveryZoneCache.Values)
        {
            if (Vector3.Distance(node.worldposition, zonePos) < 0.1f)
            {
                return true; 
            }
        }
        return false;
    }


}
