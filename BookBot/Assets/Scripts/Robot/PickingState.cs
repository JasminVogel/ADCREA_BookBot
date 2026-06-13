using UnityEngine;

public class PickingState : BaseState
{
    public PickingState(RobotController _robot)
    {
        this.robot = _robot;
    }

    public override void Enter()
    {
        Debug.Log("Robot reached the target! Entering Picking State...");

        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();

        if (gm != null && gm.pileOfBooks.Count > 0)
        {
            
            Book topBook = gm.pileOfBooks.Pop();

           
            Rigidbody rb = topBook.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }


            topBook.transform.SetParent(robot.transform);
            topBook.transform.localPosition = new Vector3(0f, 1.5f, 0f); 
            topBook.transform.localRotation = Quaternion.identity;

            Debug.Log($"Successfully POPPED {topBook.name} off the Stack!");

            int scannedUPC = topBook.barcodeID;
            
            // 2. Query the Boss's Database!
            BarcodeDatabase db = GameObject.FindFirstObjectByType<BarcodeDatabase>();
            BinarySearchTreeNode databaseResult = db.Search(scannedUPC);

            if (databaseResult != null)
            {
                Debug.Log($"BST Match! Barcode {scannedUPC} belongs to {databaseResult.targetShelf.name}, Slot {databaseResult.targetSlot}");
                
                // 3. Set the target and ask for the route
                robot.currentTargetSlot = databaseResult.targetSlot;
                robot.aStar.managerOfRobot.RequestPathToShelf(databaseResult.targetShelf);
            }
            else
            {
                robot.SwitchState(new FinishedState(robot));
            }
        }
    }

    public override void Update()
    {
        // We will add the color scanning logic here next!
    }

    public override void Exit()
    {
    }



}
