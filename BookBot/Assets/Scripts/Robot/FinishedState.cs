using UnityEngine;

public class FinishedState : BaseState
{

    public FinishedState(RobotController _robot)
    {
        this.robot = _robot;
    }

    public override void Enter()
    {
        Debug.Log("[FINISHED STATE] All books sorted! Robot is powering down.");

        // Revive the menu safely from a dedicated state!
        MenuManager menu = GameObject.FindFirstObjectByType<MenuManager>();
        if (menu != null)
        {
            Debug.Log("[FINISHED STATE] MenuManager found! Turning screen on.");
            menu.ShowMenu();
        }
        else
        {
            Debug.LogError("[CRITICAL ERROR] FinishedState cannot find the MenuManager! Is it on the Canvas?");
        }
    }

    public override void Update()
    {
        // The robot just stands still and waits for the player to press Start again!
    }

    public override void Exit()
    {
        Debug.Log("[FINISHED STATE] New simulation requested. Waking up!");
    }

}
