using UnityEngine;

public class Fort : MonoBehaviour
{
    private int _health = 100;
    public int Health=>_health;

    public delegate void OnFortDamage();
    public event OnFortDamage OnFortDamageTaken;
    public delegate void OnDestroyFort();
    public event OnDestroyFort OnFortDestroy;


    public void DamageTaken(int damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(_health, 0, 100);
        OnFortDamageTaken?.Invoke();
        if (_health <= 0)
            OnFortDestroy?.Invoke();
    }
}
