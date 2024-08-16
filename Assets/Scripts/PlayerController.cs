using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer toolSprite;
    public Image toolSpriteCanvas;
    public Sprite idleUp, idleDown, idleSide;

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
    }

    public void EquipItem(Item item) {
        toolSprite.sprite = item.itemIcon;
        toolSprite.gameObject.SetActive(true);

        toolSpriteCanvas.gameObject.SetActive(true);
        toolSpriteCanvas.sprite = item.itemIcon;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;
        isMoving = movement.sqrMagnitude > 0;
        
        if (isMoving)
        {
            lastMovementDirection = movement;
            SetWalkSprite();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
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
}
