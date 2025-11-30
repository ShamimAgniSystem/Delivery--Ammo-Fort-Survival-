using UnityEngine;

public class Fort : MonoBehaviour
{
    private int _health = 100;
    public int Health { get; set; }

    public delegate void OnFortDamage();
    public event OnFortDamage OnFortDamageTaken;
    public delegate void OnDestroyFort();
    public event OnDestroyFort OnFortDestroy;


    public void DamageTaken(int damage)
    {
        Health -= damage;
        OnFortDamageTaken?.Invoke();
        if (Health <= 0)
            OnFortDestroy?.Invoke();
    }
}
