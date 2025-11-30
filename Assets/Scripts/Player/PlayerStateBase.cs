using UnityEngine;

public interface IPlayerState
{
    void EnterState();
    void Update();
    void FixedUpdate();
    void ExitState();
}

[System.Serializable]
public abstract class PlayerStateMachine : IPlayerState
{
    protected PlayerLocomotion Controller;
    
    public virtual void Initialize(PlayerLocomotion controller)
    {
        this.Controller = controller;
    }
    
    public abstract PlayerStates StateType { get; }
    public abstract void EnterState();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void ExitState();
}