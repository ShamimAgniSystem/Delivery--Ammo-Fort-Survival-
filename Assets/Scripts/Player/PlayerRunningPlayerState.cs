using UnityEngine;

public class PlayerRunningState : PlayerStateMachine
{
    public override PlayerStates StateType => PlayerStates.Running;

    public override void EnterState() 
    {
        if (Controller.m_Animator != null)
            Controller.m_Animator.SetBool("isRunning", true);
    }

    public override void Update()
    {
        if (!Controller.IsMovingInput())
            Controller.SwitchState(PlayerStates.Idle);

        if (Controller.IsCollectInput() && Controller.IsNearArmory())
            Controller.SwitchState(PlayerStates.Collecting);
            
        if (Controller.IsDeliverInput() && Controller.IsNearSoldier())
            Controller.SwitchState(PlayerStates.Delivering);
    }

    public override void FixedUpdate() 
    {
        Controller.Move();
    }

    public override void ExitState() 
    {
        if (Controller.m_Animator != null)
            Controller.m_Animator.SetBool("isRunning", false);
    }
}