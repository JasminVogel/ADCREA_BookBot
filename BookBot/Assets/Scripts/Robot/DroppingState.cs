using UnityEngine;

public class DroppingState : BaseState
{

    private float waitTimer = 0f;
    private float timeToWait = 2f; 
    private bool hasDropped = false;
public DroppingState(RobotController robot)
    {
        this.robot = robot;
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
            //gives book to shelf (parenting removed)
            if (heldBook.myShelf != null)
            {
               
                heldBook.myShelf.AcceptBook(heldBook, robot.currentTargetSlot);
            }

            Debug.Log($"Successfully inserted {heldBook.name} into slot {robot.currentTargetSlot}!");
        }

        //done let's start over
        robot.SwitchState(new IdleState(robot));

        
        
        if (robot.gameManager != null && robot.gameManager.pileOfBooks.Count == 0)
        {
            Debug.Log("DROPPING STATE:  last book! going to FinishedState.");
            robot.SwitchState(new FinishedState(robot));
        }
        else
        {
            Debug.Log("More books. Going back to Idle to scan for the next one.");
            robot.SwitchState(new IdleState(robot));
        }


    }
    public override void Exit() 
    {}
}
