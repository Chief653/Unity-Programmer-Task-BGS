using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
