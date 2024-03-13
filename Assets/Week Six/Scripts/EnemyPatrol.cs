using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float pauseDuration = 2f;
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private bool isPaused = true;
    private float pauseTimer = 0f;

    private void Start()
    {
        MoveTowardsWaypoint(waypoints[currentWaypointIndex].position);
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

                //move to next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                MoveTowardsWaypoint(waypoints[currentWaypointIndex].position);
            }
        }
    }

    private void MoveTowardsWaypoint(Vector3 waypointPosition)
    {
        transform.LookAt(waypointPosition);
    }
}
