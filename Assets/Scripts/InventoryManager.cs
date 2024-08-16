using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> allTabs;
    public Item startItem;
    public Transform toolsParent;
    public Transform consumablesParent;
    public Transform armorsParent;
    public GameObject itemPrefab;

    [HideInInspector]
    public List<Transform> toolSlots;
    [HideInInspector]
    public List<Transform> consumableSlots;
    [HideInInspector]
    public List<Transform> armorSlots;

    void Start() {
        List<Transform> toolsParent = new List<Transform>();
        List<Transform> consumablesParent = new List<Transform>();
        List<Transform> armorsParent = new List<Transform>();

        PopulateSlots();
        NewItem(startItem);
        NewItem(startItem);
    }

    void PopulateSlots()
    {
        foreach (Transform slot in toolsParent)
        {
            toolSlots.Add(slot);
        }

        foreach (Transform slot in consumablesParent)
        {
            consumableSlots.Add(slot);
        }

        foreach (Transform slot in armorsParent)
        {
            armorSlots.Add(slot);
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
                AssignItemToSlot(newItem, toolSlots);
                break;
            case ItemType.Consumable:
                AssignItemToSlot(newItem, consumableSlots);
                break;
            case ItemType.Armor:
                AssignItemToSlot(newItem, armorSlots);
                break;
        }
    }

    void AssignItemToSlot(GameObject newItem, List<Transform> slots)
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount == 0)
            {
                newItem.transform.SetParent(slot);
                newItem.transform.localPosition = Vector3.zero;
                return;
            }
        }

        Debug.Log("No available slots!");
        //Mostrar aviso de inventário cheio
    }
}
