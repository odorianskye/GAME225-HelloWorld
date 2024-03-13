using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float pauseDuration = 2f;
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private bool isPaused = false;
    private float pauseTimer = 0f;

    private void Start()
    {
        if (waypoints.Length > 0)
        {
            MoveTowardsWaypoint(waypoints[currentWaypointIndex].position);
        }
        else
        {
            Debug.LogError("No waypoints assigned to EnemyPatrol script on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (!isPaused)
        {
            //move towards waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

            //check if enemy transform is at the waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.01f)
            {
                //pause
                isPaused = true;
                pauseTimer = pauseDuration;
            }
        }
        else
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isPaused = false;

                // Move to next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                MoveTowardsWaypoint(waypoints[currentWaypointIndex].position);
            }
        }
    }

    private void MoveTowardsWaypoint(Vector3 waypointPosition)
    {
        transform.LookAt(waypointPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage to the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }



}
