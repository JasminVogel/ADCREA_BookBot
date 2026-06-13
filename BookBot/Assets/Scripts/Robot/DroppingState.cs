using UnityEngine;

public class DroppingState : BaseState
{

    private float waitTimer = 0f;
    private float timeToWait = 2f; 
    private bool hasDropped = false;
public DroppingState(RobotController _robot)
    {
        this.robot = _robot;
    }

    public override void Enter()
    {
        waitTimer = 0f;
        hasDropped = false;
    }

    public override void Update()
    {
        if (hasDropped) return; 

      
        waitTimer += Time.deltaTime;

        
        if (waitTimer >= timeToWait)
        {
            ExecuteDropOff();
        }
    }

    private void ExecuteDropOff()
    {
        hasDropped = true; 

        Book heldBook = robot.GetComponentInChildren<Book>();
        
        if (heldBook != null)
        {
          
            heldBook.isSorted = true; 
            
            
            heldBook.transform.SetParent(null);

            if (heldBook.myShelf != null)
            {
               
                heldBook.myShelf.AcceptBook(heldBook, robot.currentTargetSlot);
            }

            Debug.Log($"Successfully inserted {heldBook.name} into slot {robot.currentTargetSlot}!");
        }

        // Job done! Go back to sleep so the radar turns on to look for the next book.
        robot.SwitchState(new IdleState(robot));

        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
        
        if (gm != null && gm.pileOfBooks.Count == 0)
        {
            Debug.Log("[DROPPING STATE] That was the last book! Transitioning to FinishedState.");
            robot.SwitchState(new FinishedState(robot));
        }
        else
        {
            Debug.Log("[DROPPING STATE] More books remain. Going back to Idle to scan for the next one.");
            robot.SwitchState(new IdleState(robot));
        }


    }
    public override void Exit() {}
}
