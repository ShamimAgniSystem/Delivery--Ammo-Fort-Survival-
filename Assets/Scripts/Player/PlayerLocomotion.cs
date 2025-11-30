using UnityEngine;
using System.Collections.Generic;

public class PlayerLocomotion : CharacterBase
{
    [Header("Player Components")]
    public CharacterController controller;
    public Animator m_Animator;

    [Header("Movement Settings")]
    public float rotationSpeed = 12f;
    
    [Header("Physics Settings")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayerMask = 1;
    public float stickToGroundForce = 0.5f;
    private Vector3 velocity;
    private bool isGrounded;
    
    [Header("State Machine")]
    private bool isSwitchingState = false;
    private Dictionary<PlayerStates, PlayerStateMachine> stateMachine = new Dictionary<PlayerStates, PlayerStateMachine>();
    [SerializeField] private PlayerStates currentStateKey = PlayerStates.Idle;
    public PlayerStates GetCurrentStateKey => currentStateKey;
    
    [Header("Interaction Settings")]
    public float ammoDelivaryAndCollectRange = 2f;
    
    private PlayerStateMachine CurrentState { 
        get
        {
            if (stateMachine.TryGetValue(currentStateKey, out PlayerStateMachine state)) return state;
            return null;
        } 
    }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        m_Animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        InitStates();
    }

    public void InitStates()
    {
        AddToStateMachine(new PlayerIdleState());
        AddToStateMachine(new PlayerRunningState());
        AddToStateMachine(new PlayerCollectingState());
        AddToStateMachine(new PlayerDeliverState());
        
        SwitchState(PlayerStates.Idle);
    }

    private void Update()
    {
        CurrentState?.Update();
    }
    
    private void FixedUpdate()
    {
        if (isSwitchingState)
        {
            isSwitchingState = false;
            return;
        }
        HandleGravityAndGround();
        CurrentState?.FixedUpdate();
    }

    void AddToStateMachine(PlayerStateMachine state)
    {
        state.Initialize(this);
        stateMachine.Add(state.StateType, state);
    }
    public void SwitchState(PlayerStates newState)
    {
        if (currentStateKey == newState) return;
        
        isSwitchingState = true;
        PlayerStateMachine prevState = CurrentState;
        currentStateKey = newState;
        prevState?.ExitState();
        CurrentState?.EnterState();
    }

    #region Inputs
    public bool IsMovingInput()
    { 
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
               Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;
    }
    
    public bool IsCollectInput()
    {
        return Input.GetKeyDown(KeyCode.C);
    }
    
    public bool IsDeliverInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    #endregion
    
    public bool IsNearArmory()
    {
        GameObject armory = GameObject.FindGameObjectWithTag("Armory");
        if (armory != null)
        {
            return Vector3.Distance(transform.position, armory.transform.position) <= ammoDelivaryAndCollectRange;
        }
        return false;
    }
    
    public bool IsNearSoldier()
    {
        if (SoldierManager.Instance == null) return false;
        
        SoldierClass nearestSoldier = SoldierManager.Instance.GetNearestSoldierNeedingAmmo(transform.position);
        return nearestSoldier != null && 
               Vector3.Distance(transform.position, nearestSoldier.transform.position) <= ammoDelivaryAndCollectRange;
    }
    private void HandleGravityAndGround()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (controller.height / 2 - controller.radius),groundCheckDistance, groundLayerMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -stickToGroundForce; 
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    public override void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;
        
        if (input.magnitude > 0)
        {
            m_Animator.SetBool("isRunning", true);
        }
        else
        {
            m_Animator.SetBool("isRunning", false);
        }
        if (controller != null)
        {
            Vector3 horizontalMovement = input * (moveSpeed * Time.deltaTime);
            controller.Move(horizontalMovement);
        }
        Rotate(input);
    }
    public void Rotate(Vector3 dir)
    {
        if (dir.magnitude == 0) return;

        Quaternion tRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tRot, Time.deltaTime * rotationSpeed);
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
}