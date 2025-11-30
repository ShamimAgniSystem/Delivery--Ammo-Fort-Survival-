using UnityEngine;
using System;

public class CharacterBase : MonoBehaviour
{
    [Header("Base Character Properties")]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected internal bool isAlive = true;
    
    public delegate void OnDamageTaken(int damage);
    public event OnDamageTaken DamageTakenEvent;
    public delegate void OnDeath();
    public event OnDeath DeathEvent;
    
    public virtual void Move(){}
    public virtual int GetCurrentHealth()
    {
        return currentHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (!isAlive) return;
        currentHealth -= damage;
        DamageTakenEvent?.Invoke(damage);
        
        if (currentHealth <= 0)
        {
            isAlive = false;
            DeathEvent?.Invoke();
        }
    }
}
public enum PlayerStates
{
    Idle,
    Running,
    Collecting,
    Delivering
}

public enum SoldierState
{
    Idle,
    Combat,
    Reloading,
    Dead
}