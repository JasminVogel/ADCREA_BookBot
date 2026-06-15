using UnityEngine;
using UnityEngine.EventSystems;

public class ObstaclePlacer : MonoBehaviour
{
    public GameObject wetFloorSignPrefab;
    public LayerMask floorLayer; 
    public FloorGrid grid; 
    public RobotManager robotManager;

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

        if (TryRemoveSign(ray))
        {
            return; 
        }

        TryPlaceSign(ray);
    }

    private bool TryRemoveSign(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Obstacle")))
        {
            GridNode nodeToFree = grid.NodeFromWorldPoint(hit.collider.transform.position);
            
            if (nodeToFree != null)
            {
                nodeToFree.isWalkable = true;
            }

            Destroy(hit.collider.gameObject);
            return true; 
        }

        return false; 
    }

    private void TryPlaceSign(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorLayer))
        {
            GridNode clickedNode = grid.NodeFromWorldPoint(hit.point);

            if (clickedNode != null && clickedNode.isWalkable)
            {
                if (IsForbiddenZone(clickedNode)) return; 

                Vector3 spawnPos = clickedNode.worldposition + (Vector3.up * 0.5f);

                if (IsSpaceOccupied(spawnPos)) return;

                Instantiate(wetFloorSignPrefab, spawnPos, Quaternion.identity);
                clickedNode.isWalkable = false;
            }
        }
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

    private bool IsSpaceOccupied(Vector3 spawnPos)
    {
        Collider[] existingSigns = Physics.OverlapSphere(spawnPos, 0.4f, LayerMask.GetMask("Obstacle"));
        return existingSigns.Length > 0;
    }
}
