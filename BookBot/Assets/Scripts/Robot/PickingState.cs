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

            robot.currentTargetSlot = robot.ScanForCorrectSlot(topBook.myShelf, topBook.myColor);
            Debug.Log($"Robot Scanner: This book belongs in Slot {robot.currentTargetSlot}!");
            robot.aStar.managerOfRobot.RequestPathToShelf(topBook.myShelf);
        }
        else
        {
            Debug.Log("The pile is completely empty! Going back to rest.");
            robot.SwitchState(new IdleState(robot));
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
