using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target; // Player or any target
    public float moveSpeed = 3f;
    public float stopDistance = 2f; // Distance to stop before reaching the player

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Ensure Rigidbody settings are correct
        if (rb != null)
        {
            rb.isKinematic = false; // Allow physics to apply
            rb.useGravity = true;   // Ensure gravity is applied
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Prevent passing through objects
        }

        if (target == null)
        {
            Debug.LogError("Target is not assigned! Assign a player or target object.");
        }
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > stopDistance)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Enemy collided with {collision.gameObject.name}");
    }
}
