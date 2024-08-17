using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Chest : MonoBehaviour
{
    public Sprite openChestSprite;
    public GameObject interactText;
    public List<Item> itemToGive;

    private bool isPlayerInRange = false;
    private bool isChestOpened = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactText.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && !isChestOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(isChestOpened) {
                interactText.SetActive(false);
                return;
            }

            isPlayerInRange = true;
            interactText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactText.SetActive(false);
        }
    }

    void OpenChest()
    {
        bool hasSpace = true;

        foreach (Item item in itemToGive)
        {
            bool slotAvailable = false;

            switch (item.itemType)
            {
                case ItemType.Tool:
                    slotAvailable = InventoryManager.instance.toolSlots.Any(slot => slot.currentItem == null);
                    break;
                case ItemType.Consumable:
                    slotAvailable = InventoryManager.instance.consumableSlots.Any(slot => slot.currentItem == null);
                    break;
                case ItemType.Armor:
                    slotAvailable = InventoryManager.instance.armorSlots.Any(slot => slot.currentItem == null);
                    break;
            }

            if (!slotAvailable)
            {
                hasSpace = false;
                break;
            }
        }

        if (!hasSpace)
        {
            InventoryManager.instance.popupNoSpace.SetActive(true);
            return;
        }

        spriteRenderer.sprite = openChestSprite;
        isChestOpened = true;
        interactText.SetActive(false);
        GiveItemToPlayer();
    }

    void GiveItemToPlayer()
    {
        foreach (Item item in itemToGive)
        {
            InventoryManager.instance.NewItem(item);
        }

        InventoryManager.instance.newItemLabel.GetComponent<SetupNewItem>().SetupDet(itemToGive);
        InventoryManager.instance.newItemLabel.SetActive(true);
    }
}
