using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 50f;
    public int damage = 10;
    public float lifeTime = 3f;
    
    [Header("Effects")]
    public GameObject hitEffect;
    
    private Transform target;
    private bool hasHit = false;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (hasHit) return;
        transform.position += transform.forward * (speed * Time.deltaTime);

        if (target != null && target.gameObject.activeInHierarchy)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyClass enemy = other.GetComponent<EnemyClass>();
            if (enemy != null && enemy.isAlive)
            {
                enemy.TakeDamage(damage);
                HitTarget();
            }
        }
        else if (!other.CompareTag("Soldier") && !other.CompareTag("Player") && !other.CompareTag("Bullet"))
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        hasHit = true;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}