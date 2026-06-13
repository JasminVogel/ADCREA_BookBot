using UnityEngine;

public class DroppingState : BaseState
{
public DroppingState(RobotController _robot)
    {
        this.robot = _robot;
    }

    public override void Enter()
    {
        Debug.Log("Arrived at the Delivery Zone! Entering Dropping State...");

       
        Book heldBook = robot.GetComponentInChildren<Book>();
        
        if (heldBook != null)
        {
            heldBook.isSorted = true;
            heldBook.transform.SetParent(null);

            
            if (heldBook.myShelf != null)
            {
               heldBook.myShelf.AcceptBook(heldBook, robot.currentTargetSlot);

            Debug.Log($"Successfully delivered {heldBook.name}!");
            }

        }
        robot.SwitchState(new IdleState(robot));
    }

    public override void Update() {}
    public override void Exit() {}
}
