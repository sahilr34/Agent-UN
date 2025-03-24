using UnityEngine;
using System.Collections;

public class Mushroom : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;

    [Header("Collision Settings")]
    public LayerMask wallLayer;
    public LayerMask playerLayer;
    public float raycastDistance = 0.1f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private BoxCollider2D coll;

    private Vector2 currentDirection;
    private bool isStopped = false;
    private bool isChasing = false;
    private Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        currentDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        CheckForPlayer();
        HandleMovement();
    }

    void Update()
    {
        animator.SetBool("isWalking", Mathf.Abs(rb.linearVelocity.x) > 0.1f && !isStopped);
    }

    void CheckForPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerCollider != null && playerCollider.CompareTag("Player"))
        {
            isChasing = true;
            playerTransform = playerCollider.transform;

            if (isStopped)
            {
                StopAllCoroutines();
                isStopped = false;
            }
        }
        else
        {
            isChasing = false;
            playerTransform = null;
        }
    }

    void HandleMovement()
    {
        if (isStopped) return;

        if (isChasing && playerTransform != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer()
    {
        float directionX = Mathf.Sign(playerTransform.position.x - transform.position.x);
        currentDirection = new Vector2(directionX, 0);
        sr.flipX = currentDirection.x < 0;
        rb.linearVelocity = new Vector2(currentDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    void Patrol()
    {
        Vector2 rayOrigin = (Vector2)transform.position + currentDirection * coll.bounds.extents.x;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, currentDirection, raycastDistance, wallLayer);

        if (hit.collider != null)
        {
            StartCoroutine(StopAndFlip());
        }
        else
        {
            rb.linearVelocity = new Vector2(currentDirection.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    IEnumerator StopAndFlip()
    {
        isStopped = true;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1f);

        sr.flipX = !sr.flipX;
        currentDirection *= -1;
        isStopped = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}