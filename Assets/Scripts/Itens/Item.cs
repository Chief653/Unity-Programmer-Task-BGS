using UnityEngine;
using NaughtyAttributes;

public enum ItemType
{
    Tool,
    Consumable,
    Armor
}

public enum ConsumableType { Health, Speed, ExtraDamage }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int itemId;
    public float value;
    public float timeEffect;
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;
    public ItemType itemType;

    [ShowIf("IsConsumable")]
    public ConsumableType consumableType;

    private bool IsConsumable()
    {
        return itemType == ItemType.Consumable;
    }
}
