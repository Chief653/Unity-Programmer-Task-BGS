using UnityEngine;

public enum ItemType
{
    Tool,
    Consumable,
    Armor
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int itemId;
    public string itemName;
    public string itemDesc;
    public ItemType itemType;
    public Sprite itemIcon;
}
