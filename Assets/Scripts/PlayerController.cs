using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer toolSprite;
    public Image toolSpriteCanvas, consSpriteCanvas, consSpriteShadowCanvas;
    public Sprite idleUp, idleDown, idleSide;
    public bool isDead = false;
    
    public Slider healthSlider;
    public GameObject defeatScreen;
    private float maxHealth = 100f;
    private float currentHealth;
    private Tween healthTween, fillTween;

    private Rigidbody2D rb;
    private Animator animPlayer;
    private Vector2 movement;
    private Vector2 lastMovementDirection;
    private bool isMoving;

    void Awake() {
        instance = this;
    }

    void Start() {
        animPlayer = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        defeatScreen.SetActive(false);
    }

    public void EquipItem(Item item) {
        if(item.itemType == ItemType.Tool) { 
            toolSprite.sprite = item.itemIcon;
            toolSprite.gameObject.SetActive(true);

            toolSpriteCanvas.sprite = item.itemIcon;
            toolSpriteCanvas.gameObject.SetActive(true);
        }
        else {
            consSpriteCanvas.sprite = item.itemIcon;
            consSpriteCanvas.gameObject.SetActive(true);

            consSpriteShadowCanvas.sprite = item.itemIcon;
            consSpriteShadowCanvas.gameObject.SetActive(true);
        }
    }

    public void UnequipItem(Item item) {
        if(item.itemType == ItemType.Tool) { 
            toolSprite.gameObject.SetActive(false);
            toolSpriteCanvas.gameObject.SetActive(false);
        }
        else if(item.itemType == ItemType.Consumable) {
            consSpriteCanvas.gameObject.SetActive(false);
            consSpriteShadowCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(isDead)
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;
        isMoving = movement.sqrMagnitude > 0;
        
        if (isMoving)
        {
            lastMovementDirection = movement;
            SetWalkSprite();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {

            if(InventoryManager.instance.equippedConsumable != null) {
                if(InventoryManager.instance.equippedConsumable.consumableType == ConsumableType.Health) {
                    if(currentHealth >= maxHealth)
                        return;

                    ChangeHealth(InventoryManager.instance.equippedConsumable.value);
                }
                else if(InventoryManager.instance.equippedConsumable.consumableType == ConsumableType.Speed) {
                    ModifyPlayerSpeed(InventoryManager.instance.equippedConsumable.value, InventoryManager.instance.equippedConsumable.timeEffect);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(isDead)
            return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void LateUpdate() {
        if(!isMoving) {
            SetIdleSprite();
        }
    }

    void SetIdleSprite()
    {
        animPlayer.SetInteger("Direction", 0);

        if (lastMovementDirection.y > 0) {
            spriteRenderer.sprite = idleUp;
        }
        else if (lastMovementDirection.y < 0) {
            spriteRenderer.sprite = idleDown;
        }
        else if (lastMovementDirection.x != 0)
        {
            spriteRenderer.sprite = idleSide;
            spriteRenderer.flipX = lastMovementDirection.x < 0;
        }
    }

    void SetWalkSprite()
    {
        if (movement.y > 0) {
            animPlayer.SetInteger("Direction", 3);
        }
        else if (movement.y < 0) {
            animPlayer.SetInteger("Direction", 2);
        }
        else if (movement.x != 0)
        {
            animPlayer.SetInteger("Direction", 1);
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    public void ChangeHealth(float amount)
    {
        if(healthTween.IsActive())
            return;

        if (fillTween != null && fillTween.IsActive())
        {
            fillTween.Kill();
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthTween = DOTween.To(() => healthSlider.value, x => healthSlider.value = x, currentHealth, 1f);

        if(amount > 0) {
            consSpriteCanvas.fillAmount = 0f;
            fillTween = DOTween.To(() => consSpriteCanvas.fillAmount, x => consSpriteCanvas.fillAmount = x, 1f, 1f);
        }

        if (currentHealth <= 0)
        {
            isMoving = false;
            isDead = true;
            rb.velocity = Vector3.zero;
            defeatScreen.SetActive(true);
        }
    }

    public void ModifyPlayerSpeed(float speedMultiplier, float duration)
    {
        if (fillTween != null && fillTween.IsActive())
            return;

        moveSpeed *= speedMultiplier;

        consSpriteCanvas.fillAmount = 0f;
        fillTween = DOTween.To(() => consSpriteCanvas.fillAmount, x => consSpriteCanvas.fillAmount = x, 1f, duration);

        DOVirtual.DelayedCall(duration, () =>
        {
            moveSpeed /= speedMultiplier;

            if (fillTween.IsActive())
            {
                fillTween.Kill();
            }
        });
    }
}
