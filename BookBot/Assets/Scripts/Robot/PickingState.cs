using UnityEngine;

public class PickingState : BaseState
{
    private BinarySearchTreeNode foundBookData;
    public PickingState(RobotController robot)
    {
        this.robot = robot;
    }

    public override void Enter()
    {
        Debug.Log("Robot reached the target => Picking State...");


        if(robot.gameManager != null && robot.gameManager.pileOfBooks.Count > 0)
        {
            Book topBook = robot.gameManager.pileOfBooks.Pop();

           
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

            BinarySearchTreeNode databaseResult = robot.database.Search(scannedUPC);


            if (databaseResult != null)
            {
                Debug.Log("BST Match! Barcode " + scannedUPC + " belongs to " + databaseResult.targetShelf.name + ", Slot " + databaseResult.targetSlot);
                
                if (robot.visualizer != null && robot.database.lastSearchPath.Count > 0)
                {
                    Debug.Log("Playing BST Hologram Animation!");
                    
                    
                    foundBookData = databaseResult;

                    
                    robot.visualizer.PlayHologramAnimation(robot.transform.position, robot.database.lastSearchPath, OnHologramFinished);
                }
                else
                {
                    Debug.LogWarning("[PICKING STATE] Visualizer skipped. Walking normally.");
                    robot.currentTargetSlot = databaseResult.targetSlot;
                    robot.aStar.managerOfRobot.RequestPathToShelf(databaseResult.targetShelf);
                }
            }   
            else
            {
                robot.SwitchState(new FinishedState(robot));
            }
        }
    
    }

    private void OnHologramFinished()
    {
        robot.currentTargetSlot = foundBookData.targetSlot;
        robot.aStar.managerOfRobot.RequestPathToShelf(foundBookData.targetShelf);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
    }



}
