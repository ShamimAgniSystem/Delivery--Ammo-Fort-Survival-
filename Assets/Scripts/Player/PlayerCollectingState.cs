using UnityEngine;

public class PlayerCollectingState : PlayerStateMachine
{
    public override PlayerStates StateType => PlayerStates.Collecting;

    public override void EnterState()
    {
        Debug.Log("Collecting Ammo from Armory...");
        
        if (AmmoSystem.Instance != null)
        {
            AmmoSystem.Instance.CollectAmmo();
        }
        
        Controller.SwitchState(PlayerStates.Idle);
    }

    public override void Update() {}
    public override void FixedUpdate() {}
    public override void ExitState() {}
}