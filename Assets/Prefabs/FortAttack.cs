using UnityEngine;

public class FortAttack : MonoBehaviour
{
    [Header("Fort Attack")]
    public Transform handTransform;
    public float sphereRadius = 0.5f;
    public LayerMask fortMask;
    public float castDistance = 1f;
    
    public void CheckHandCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(handTransform.position, sphereRadius);

        if (hitColliders.Length > 0)
        {
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.GetComponent<Fort>().Health -= 10;
                Debug.Log("Hit On Fort !");
            }
        }
        else
        {
            Debug.Log("No collision detected.");
        }
    }
}
