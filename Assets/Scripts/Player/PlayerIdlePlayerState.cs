using UnityEngine;

public class PlayerIdleState : PlayerStateMachine
{
    public override PlayerStates StateType => PlayerStates.Idle;

    public override void EnterState() 
    {
        if (Controller.m_Animator != null)
            Controller.m_Animator.SetBool("isRunning", false);
    }

    public override void Update()
    {
        if (Controller.IsMovingInput())
            Controller.SwitchState(PlayerStates.Running);

        if (Controller.IsCollectInput() && Controller.IsNearArmory())
            Controller.SwitchState(PlayerStates.Collecting);
            
        if (Controller.IsDeliverInput() && Controller.IsNearSoldier())
            Controller.SwitchState(PlayerStates.Delivering);
    }

    public override void FixedUpdate() {}
    public override void ExitState() {}
}