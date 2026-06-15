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
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, floorLayer))
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
       
        if (activeSigns.ContainsKey(node))
        {
        
            Destroy(activeSigns[node]); 
            
      
            activeSigns.Remove(node);   
            
       
            node.isWalkable = true;     
            Debug.Log(" varible removed tile  ");
        }
        else
        {
      
            Debug.Log("This is an unwalkable tile");
        }
    }

    private void TryPlaceSignAtNode(GridNode node)
    {
        if (IsForbiddenZone(node)) return; 

        Vector3 spawnPos = node.worldPosition + (Vector3.up * 0.5f);

       
        GameObject newSign = Instantiate(wetFloorSignPrefab, spawnPos, Quaternion.identity);
        
    
        activeSigns.Add(node, newSign);
        
        node.isWalkable = false;
        Debug.Log(" sign deplyed also registred");
    }

    private bool IsForbiddenZone(GridNode node)
    {
        foreach (Vector3 zonePos in robotManager.shelfDeliveryZone.Values)
        {
            if (Vector3.Distance(node.worldPosition, zonePos) < 0.1f)
            {
                return true; 
            }
        }
        return false;
    }


}
