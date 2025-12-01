using UnityEngine;
using System.Collections.Generic;

public class SoldierManager : MonoBehaviour
{
    public static SoldierManager Instance;

    [Header("Soldier Management")]
    [SerializeField] private List<SoldierClass> allSoldiers = new List<SoldierClass>();
    [SerializeField] private Transform[] soldierSpawnPoints;
    public List<SoldierClass> AllSoldiers => allSoldiers;
    public List<SoldierClass> SoldiersNeedingAmmo 
    {
        get
        {
            List<SoldierClass> needySoldiers = new List<SoldierClass>();
        
            foreach (SoldierClass soldier in allSoldiers)
            {
                if (soldier.NeedsAmmo)
                {
                    needySoldiers.Add(soldier);
                }
            }
            return needySoldiers;
        }
    }

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

    private void OnSoldierAmmoLow(SoldierClass soldier)
    {
        Debug.Log($"Warning: {soldier.soldierName} is low on ammo!");
    }

    private void OnSoldierAmmoEmpty(SoldierClass soldier)
    {
        Debug.Log($"URGENT: {soldier.soldierName} is out of ammo!");
    }

    private void OnSoldierDead(SoldierClass soldier)
    {
        allSoldiers.Remove(soldier);
        Debug.Log($"Soldier {soldier.soldierName} has been removed from duty!");

        if (allSoldiers.Count == 0)
        {
            Debug.Log("All soldiers have fallen! Game Over!");
        }
    }

    public SoldierClass GetNearestSoldierNeedingAmmo(Vector3 playerPosition)
    {
        List<SoldierClass> needySoldiers = SoldiersNeedingAmmo;
        if (needySoldiers.Count == 0) return null;

        SoldierClass nearest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var soldier in needySoldiers)
        {
            if (soldier == null) continue;
            
            float distance = Vector3.Distance(playerPosition, soldier.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = soldier;
            }
        }
        return nearest;
    }

    private void OnDestroy()
    {
        foreach (var soldier in allSoldiers)
        {
            if (soldier != null)
            {
                soldier.OnAmmoLow -= OnSoldierAmmoLow;
                soldier.OnAmmoEmpty -= OnSoldierAmmoEmpty;
            }
        }
    }
}