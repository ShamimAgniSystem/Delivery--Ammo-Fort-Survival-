using UnityEngine;
using System;

public class SoldierClass : CharacterBase
{
    [Header("Combat Settings")]
    public string soldierName = "Soldier";
    public int maxAmmo = 30;
    public int currentAmmo;
    public float attackRange = 15f;
    public float fireRate = 1f;
    public int damagePerShot = 10;
    public float detectionRadius = 10f;
    public LayerMask enemyLayerMask;
    
    [Header("Bullet System")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    
    [Header("Effects & Audios")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource gunAudio;
    
    [Header("Soldier Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private float rotationSpeed = 5f;
    
    
    private float fireCooldown = 0f;
    private EnemyClass currentTarget;
    private SoldierState currentState = SoldierState.Idle;
    private Quaternion originalRotation;
    private bool hasLockedTarget = false;
    private float targetSearchCooldown = 0f;
    private float targetSearchInterval = 0.5f;

    public EnemyClass CurrentTarget => currentTarget;
    public bool HasAmmo => currentAmmo > 0;
    public bool NeedsAmmo => currentAmmo < maxAmmo * 0.3f;
    public SoldierState CurrentState => currentState;
    public string SoldierName => soldierName;

    public Action<SoldierClass> OnAmmoLow;
    public Action<SoldierClass> OnAmmoEmpty;

    protected void Awake()
    {
        Initialize();
        originalRotation = transform.rotation;
        
        if (bulletSpawnPoint == null)
            bulletSpawnPoint = transform;
    }

    public void Initialize()
    {
        currentAmmo = maxAmmo;
        isAlive = true;
        currentHealth = maxHealth;
        hasLockedTarget = false;
    }

    private void Update()
    {
        if (!isAlive) return;

        CheckCurrentTarget();

        // Search for new targets periodically
        targetSearchCooldown -= Time.deltaTime;
        if (targetSearchCooldown <= 0f)
        {
            FindNearestEnemy();
            targetSearchCooldown = targetSearchInterval;
        }

        UpdateStateMachine();
        UpdateAnimations();

        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;
    }

    private void CheckCurrentTarget()
    {
        if (currentTarget != null && !currentTarget.isAlive)
        {
            Debug.Log($"{soldierName} target destroyed, searching for new target...");
            hasLockedTarget = false;
            currentTarget = null;
            FindNearestEnemy();
        }
    }
    
    private void UpdateStateMachine()
    {
        switch (currentState)
        {
            case SoldierState.Idle:
                IdleBehavior();
                break;
            case SoldierState.Combat:
                CombatBehavior();
                break;
            case SoldierState.Reloading:
                ReloadBehavior();
                break;
            case SoldierState.Dead:
                DeadBehavior();
                break;
        }
    }
    private void IdleBehavior()
    {
        // Return to original rotation
        if (transform.rotation != originalRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }

        // Switch to combat if we have a target
        if (currentTarget != null && currentTarget.isAlive)
        {
            currentState = SoldierState.Combat;
            hasLockedTarget = true;
            Debug.Log($"{soldierName} locked target: {currentTarget.name}");
        }
    }

    private void CombatBehavior()
    {
        // Check if current target is still valid
        if (currentTarget == null || !currentTarget.isAlive)
        {
            hasLockedTarget = false;
            currentTarget = null;
            currentState = SoldierState.Idle;
            return;
        }

        // Rotate towards the locked target
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Check if target is still in range
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distanceToTarget > attackRange)
        {
            // Target out of range, but keep locked
            return;
        }

        // Shoot if possible
        if (CanShoot())
        {
            Shoot();
            
            // Check ammo status
            if (!HasAmmo)
            {
                OnAmmoEmpty?.Invoke(this);
                currentState = SoldierState.Reloading;
            }
            else if (NeedsAmmo)
            {
                OnAmmoLow?.Invoke(this);
            }
        }
    }
    private bool HasLineOfSightToTarget()
    {
        if (currentTarget == null) return false;
    
        Vector3 direction = (currentTarget.transform.position - bulletSpawnPoint.position).normalized;
        float distance = Vector3.Distance(bulletSpawnPoint.position, currentTarget.transform.position);
    
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit, distance))
        {
            // Check if the hit object is our target or another enemy
            EnemyClass hitEnemy = hit.collider.GetComponent<EnemyClass>();
            if (hitEnemy != null && hitEnemy == currentTarget)
            {
                return true; // We have direct line of sight to our target
            }
            else
            {
                // Hit something else (wall, obstacle, etc.)
                Debug.Log($"{soldierName} line of sight blocked by: {hit.collider.name}");
                return false;
            }
        }
    
        return true; // No obstacles found
    }
    private void ReloadBehavior()
    {
        // Return to original rotation while reloading
        if (transform.rotation != originalRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }

        // Check if target died while reloading
        if (currentTarget != null && !currentTarget.isAlive)
        {
            hasLockedTarget = false;
            currentTarget = null;
            currentState = SoldierState.Idle;
        }

        // Return to combat if we have ammo and target
        if (HasAmmo && !NeedsAmmo)
        {
            if (currentTarget != null && currentTarget.isAlive)
            {
                currentState = SoldierState.Combat;
            }
            else
            {
                currentState = SoldierState.Idle;
            }
        }
    }

    private void DeadBehavior()
    {
        hasLockedTarget = false;
        currentTarget = null;
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        animator.SetBool("isShooting", currentState == SoldierState.Combat && CanShoot());
        animator.SetBool("isReloading", currentState == SoldierState.Reloading);
        animator.SetBool("isDead", currentState == SoldierState.Dead);
    }

    public void ReceiveAmmo(int ammoAmount)
    {
        Reload(ammoAmount);
        
        if (currentState == SoldierState.Reloading && HasAmmo)
        {
            if (currentTarget != null && currentTarget.isAlive)
            {
                currentState = SoldierState.Combat;
            }
            else
            {
                currentState = SoldierState.Idle;
            }
        }
    }

    public EnemyClass FindNearestEnemy()
    {
        // If we already have a locked target that's alive, don't look for new ones
        if (hasLockedTarget && currentTarget != null && currentTarget.isAlive)
        {
            return currentTarget;
        }

        // Use SphereCast to find enemies
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRadius, Vector3.forward, 0.1f, enemyLayerMask);

        EnemyClass nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            EnemyClass enemy = hit.collider.GetComponent<EnemyClass>();
            if (enemy == null || !enemy.isAlive)
                continue;

            float distance = Vector3.Distance(transform.position, hit.point);
            
            // Check if enemy is in attack range
            if (distance < closestDistance && distance <= attackRange)
            {
                nearestEnemy = enemy;
                closestDistance = distance;
            }
        }

        // Set new target if found
        if (nearestEnemy != null)
        {
            currentTarget = nearestEnemy;
            hasLockedTarget = true;
            Debug.Log($"{soldierName} found new target: {nearestEnemy.name}");
        }
        else if (currentTarget == null)
        {
            // No targets found, stay in idle
            hasLockedTarget = false;
            currentState = SoldierState.Idle;
        }

        return currentTarget;
    }

    public override void Move()
    {
        // Soldiers are stationary
    }

    public override int GetCurrentHealth() => currentHealth;

    public bool CanShoot()
    {
        if (currentTarget == null) return false;
        if (!currentTarget.isAlive) return false;
        if (!HasAmmo) return false;
        if (fireCooldown > 0) return false;

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance > attackRange) return false;

        // Line of sight check
        if (!HasLineOfSightToTarget())
        {
            return false;
        }

        return true;
    }

    public void Shoot()
    {
        if (!CanShoot()) return;

        currentAmmo--;
        fireCooldown = fireRate;

        // Visual and audio effects
        if (muzzleFlash != null) muzzleFlash.Play();
        if (gunAudio != null) gunAudio.Play();

        // Create bullet
        if (bulletPrefab != null && currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - bulletSpawnPoint.position).normalized;
            CreateBullet(direction);
        }
        
        Debug.Log($"{soldierName} shooting at target: {currentTarget.name}! Ammo left: {currentAmmo}");
    }

    private void CreateBullet(Vector3 direction)
    {
        if (bulletSpawnPoint == null) return;

        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        
        if (bullet != null)
        {
            bullet.damage = damagePerShot;
        }
    }

    public void Reload(int ammoAmount)
    {
        currentAmmo += ammoAmount;
        if (currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;
        Debug.Log($"{soldierName} reloaded! Ammo: {currentAmmo}");
    }
}