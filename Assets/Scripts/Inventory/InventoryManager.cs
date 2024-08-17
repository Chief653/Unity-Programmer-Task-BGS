using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<GameObject> allTabs;

    [Tooltip("Starting Item")]
    public Item startItem;

    [Tooltip("Current equipped item")]
    public Item equippedTool, equippedConsumable, equippedArmor;
    public Transform toolsParent;
    public Transform consumablesParent;
    public Transform armorsParent;
    public GameObject itemPrefab;
    public GameObject detailsObj;
    public GameObject newItemLabel;
    public GameObject deleteConfirmationPanel;

    [HideInInspector]
    public List<Slot> toolSlots;
    [HideInInspector]
    public List<Slot> consumableSlots;
    [HideInInspector]
    public List<Slot> armorSlots;

    void Awake() {
        instance = this;

        List<Slot> toolsParent = new List<Slot>();
        List<Slot> consumablesParent = new List<Slot>();
        List<Slot> armorsParent = new List<Slot>();

        PopulateSlots();
        NewItem(startItem);
        gameObject.SetActive(false);
    }

    public void OpenInventory() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    void PopulateSlots()
    {
        foreach (Transform slot in toolsParent)
        {
            toolSlots.Add(slot.GetComponent<Slot>());
        }

        foreach (Transform slot in consumablesParent)
        {
            consumableSlots.Add(slot.GetComponent<Slot>());
        }

        foreach (Transform slot in armorsParent)
        {
            armorSlots.Add(slot.GetComponent<Slot>());
        }
    }

    public void OpenTab(GameObject activeTab) {
        foreach (GameObject tab in allTabs)
        {
            tab.SetActive(false);
        }

        activeTab.SetActive(true);
    }

    public void NewItem(Item item)
    {
        GameObject newItem = Instantiate(itemPrefab);
        newItem.name = item.itemName;
        newItem.GetComponent<Image>().sprite = item.itemIcon;

        switch (item.itemType)
        {
            case ItemType.Tool:
                AssignItemToSlot(newItem, item, toolSlots);
                break;
            case ItemType.Consumable:
                AssignItemToSlot(newItem, item, consumableSlots);
                break;
            case ItemType.Armor:
                AssignItemToSlot(newItem, item, armorSlots);
                break;
        }
    }

    void AssignItemToSlot(GameObject newItem, Item item, List<Slot> slots)
    {
        foreach (Slot slot in slots)
        {
            if (slot.currentItem == null)
            {
                slot.SetItem(item, newItem);
                return;
            }
        }

        Destroy(newItem);
    }
}
