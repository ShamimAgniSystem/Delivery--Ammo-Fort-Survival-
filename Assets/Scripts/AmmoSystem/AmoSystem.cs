using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    public static AmmoSystem Instance;

    [Header("Player Ammo Settings")]
    public int playerCarry = 0;
    public int carryLimit = 5;
    public int totalAmmoInArmory = 50;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectAmmo()
    {
        if (playerCarry < carryLimit && totalAmmoInArmory > 0)
        {
            int ammoToTake = Mathf.Min(carryLimit - playerCarry, totalAmmoInArmory);
            playerCarry += ammoToTake;
            totalAmmoInArmory -= ammoToTake;
            
            Debug.Log($"Player collected {ammoToTake} ammo. Total carrying: {playerCarry}");
        }
        else
        {
            Debug.Log("Cannot collect ammo - inventory full or armory empty!");
        }
    }

    public void TryDeliverAmmo(Transform playerPos, float distance)
    {
        if (playerCarry <= 0)
        {
            Debug.Log("No ammo to deliver!");
            return;
        }

        if (SoldierManager.Instance == null) return;

        SoldierClass targetSoldier = SoldierManager.Instance.GetNearestSoldierNeedingAmmo(playerPos.position);
        
        if (targetSoldier != null && Vector3.Distance(playerPos.position, targetSoldier.transform.position) < distance)
        {
            DeliverToSoldier(targetSoldier, playerCarry);
            Debug.Log("Delivar Amo");
            playerCarry = 0;
        }
        else
        {
            Debug.Log("No nearby soldier needs ammo!");
        }
    }

    private void DeliverToSoldier(SoldierClass soldier, int ammoAmount)
    {
        soldier.ReceiveAmmo(ammoAmount);
        Debug.Log($"Delivered {ammoAmount} ammo");
    }
}