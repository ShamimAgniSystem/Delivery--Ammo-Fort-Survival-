using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyClass : CharacterBase
{
    [Header("Enemy Settings")]
    [SerializeField] private int damageToFort = 10;
    [SerializeField] private float attackRange = 1.5f;
    
    [SerializeField] private Transform fortCenter;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    
    private void Awake()
    {
        fortCenter = GameObject.FindGameObjectWithTag("Fort").transform;
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        DeathEvent += OnDeathCallBack;
        
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = moveSpeed;
        }
    }
    private void Start()
    {
        if (fortCenter != null && navMeshAgent != null)
        {
            navMeshAgent.SetDestination(fortCenter.position);
        }
    }
    private void Update()
    {
        if (!isAlive) return;
        CheckAttackRange();
    }
    private void CheckAttackRange()
    {
        if (fortCenter == null) return;
        Move();
    }
    public override void Move() 
    { 
        float distanceToFort = Vector3.Distance(transform.position, fortCenter.position);
        if (distanceToFort <= attackRange)
        {
            transform.LookAt(fortCenter);
            animator.SetBool("isAttack", true);
            navMeshAgent.isStopped = true; 
        }
        else
        {
            animator.SetBool("isAttack", false);
            navMeshAgent.isStopped = false;
        }
    }
    private void OnDeathCallBack()
    {
        Destroy(this.gameObject);
        EnemyWave.Instance.OnEnemyKilled();
    }
    public override int GetCurrentHealth() => currentHealth;
}