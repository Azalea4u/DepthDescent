using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected float movementSpeed = 3.0f;
    [SerializeField] protected float facingDirection;
    [SerializeField] protected float initialDelayTime = 0.25f;
    [SerializeField] public int minDroppedGold;
    [SerializeField] public int maxDroppedGold;

    public bool hasTarget;
    public bool isAlive = true;
    public bool playerHit = false;
    public bool isHit = false;

    [Header("Colliders")]
    [SerializeField] protected Collider2D GroundCollider;
    [SerializeField] protected Collider2D FrontCollider;
    [SerializeField] protected Collider2D CliffCollider;
    [SerializeField] protected Collider2D DetectPlayerCollider;
    [SerializeField] protected LayerMask whatIsGround;

    protected bool isGrounded;

    protected bool canFlip = true;
    protected bool facingRight = true;
    public bool canMove = false;

    [Header("Loot Drop")]
    [SerializeField] private GameObject coinDrop;
    protected bool lootDropped;

    // STAGGER
    [SerializeField] protected float knockbackForce = 10.0f;
    [SerializeField] protected float staggerDuration = 0.5f;
    protected bool isStaggered = false;

    public Rigidbody2D rb;
    public static EnemyBase instance;

    private void Awake()
    {
        instance = this;
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(InitialDelay());
    }

    protected virtual void Update()
    {
        CheckCollision();

        animator.SetBool("IsAlive", isAlive);
        animator.SetBool("IsMoving", facingDirection != 0);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("HasTarget", hasTarget);
    }

    protected virtual void FixedUpdate()
    {
        UpdateFacingDirection();

        if (!isStaggered && isAlive && canMove)
        {
            Move();
        }
    }

    protected virtual void UpdateFacingDirection()
    {
        facingDirection = rb.linearVelocity.x;
    }

    protected virtual void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(GroundCollider.bounds.center, Vector2.down, GroundCollider.bounds.size.x, whatIsGround);

        if (CliffCollider != null)
        {
            if (FrontCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))
                || !CliffCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                FlipDirection();
            }
        }
        else
        {
            if (FrontCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                FlipDirection();
            }
        }
    }

    protected IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(initialDelayTime);
        canMove = true;
    }

    protected virtual void Move()
    {
        if (canMove)
        {
            float direction = facingRight ? 1 : -1;
            rb.linearVelocity = new Vector2(movementSpeed * direction, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    protected virtual void StopMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        canFlip = false;
    }

    protected virtual void FlipDirection()
    {
        if (!canFlip) return;

        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void Stagger(Vector2 knockbackDirection)
    {
        if (!isAlive) return;

        isHit = true;
        EnemyAudio_Play.instance.Play_IsHit();
        rb.linearVelocity = new Vector2(knockbackDirection.x * knockbackForce, rb.linearVelocity.y);
        StartCoroutine(StaggerCoroutine());
    }

    protected virtual IEnumerator StaggerCoroutine()
    {
        isStaggered = true;
        yield return new WaitForSeconds(staggerDuration);
        isStaggered = false;
        isHit = false;
    }

    public void DropLoot()
    {
        //random dropped gold between min and max
        int droppedGold = Random.Range(minDroppedGold, maxDroppedGold);

        //Debug.Log("Dropped " + droppedGold + " gold");
        // Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);

        coinDrop.GetComponent<Coin_Pickup>().gold = droppedGold;
        Instantiate(coinDrop, transform.position, Quaternion.identity);

        lootDropped = true;
    }
}
