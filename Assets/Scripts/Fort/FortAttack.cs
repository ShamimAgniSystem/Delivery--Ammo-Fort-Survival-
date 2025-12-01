using System;
using UnityEngine;

public class FortAttack : MonoBehaviour
{
    [Header("Fort Attack")]
    [SerializeField] Transform handTransform;
    [SerializeField] float sphereRadius = 0.5f;
    [SerializeField] LayerMask fortMask;
    [SerializeField] float castDistance = 1f;
    
    [Header("References")]
    [SerializeField] private Fort fort;

    private void Awake()
    {
        if (fort == null)
            fort = GameObject.FindGameObjectWithTag("Fort").GetComponent<Fort>();
    }
    private void Start()
    {
        fort.OnFortDamageTaken += FortDamageCallBack;
        fort.OnFortDestroy += FortDestroyCallBack;
    }
    public void AttackingFort()
    {
        Collider[] hitColliders = Physics.OverlapSphere(handTransform.position, sphereRadius, fortMask);
        if (hitColliders.Length > 0)
        {
            foreach (var hitCollider in hitColliders)
            {
                if (fort != null)
                {
                    fort.DamageTaken(10);
                }
            }
        }
    }

    private void FortDamageCallBack()
    {
        Debug.Log("Fort Damage Taken! " + fort.Health);
    }
    private void FortDestroyCallBack()
    {
        Debug.Log("Fort Destroyed!");
    }
    private void OnDestroy()
    {
        fort.OnFortDamageTaken -= FortDamageCallBack;
        fort.OnFortDestroy -= FortDestroyCallBack;
    }
}