using UnityEngine;

public class PlayerDeliverState : PlayerStateMachine
{
    public override PlayerStates StateType => PlayerStates.Delivering;

    public override void EnterState()
    {
        Debug.Log("Delivering Ammo to Soldier...");
        
        if (AmmoSystem.Instance != null)
        {
            AmmoSystem.Instance.TryDeliverAmmo(Controller.transform,Controller.ammoDelivaryAndCollectRange);
        }
        Controller.SwitchState(PlayerStates.Idle);
    }

    public override void Update() {}
    public override void FixedUpdate() {}
    public override void ExitState() {}
}