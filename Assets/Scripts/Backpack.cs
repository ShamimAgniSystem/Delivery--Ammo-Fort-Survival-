using UnityEngine;

[CreateAssetMenu (fileName = "Backpack", menuName = "Inventory/Backpack")]
public class Backpack : ScriptableObject
{
    [SerializeField] private float backpackSize;
}