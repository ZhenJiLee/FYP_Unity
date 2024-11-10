using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blade : MonoBehaviour
{
    public int baseDamage = 30;
    public GameObject[] objectsToToggle;
    public float interval = 2f;
    public float moveSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public List<Transform> waypoints;

    private Animator animator;
    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (objectsToToggle.Length > 0)
        {
            StartCoroutine(ToggleObjectsAndAnimate());
        }
        else
        {
            Debug.LogWarning("No objects assigned to toggle.");
        }

        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints assigned for the Blade.");
        }
    }

    private void FixedUpdate()
    {
        if (waypoints.Count > 0)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Count)
        {
            currentWaypointIndex = 0;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        float distance = Vector2.Distance(targetWaypoint.position, transform.position);
        if (distance <= waypointReachedDistance)
        {
            currentWaypointIndex++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            DamagePlayer(player);
        }
    }

    private void DamagePlayer(PlayerController player)
    {
        Damageable damageable = player.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 knockback = Vector2.zero; 
            damageable.Hit(baseDamage, knockback);
            Debug.Log("Player hit by BladeSpike, damage: " + baseDamage);
        }
    }

    private IEnumerator ToggleObjectsAndAnimate()
    {
        while (true)
        {
            
            foreach (GameObject obj in objectsToToggle)
            {
                obj.SetActive(!obj.activeSelf);
            }
            
            if (animator != null)
            {
                animator.SetTrigger("ToggleAnimation");
            }
            yield return new WaitForSeconds(interval);
        }
    }
}