using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite openChestSprite;
    public GameObject interactText;
    public Item itemToGive;

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
        InventoryManager.instance.NewItem(itemToGive);
    }
}
