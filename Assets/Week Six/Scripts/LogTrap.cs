using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTrap : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint; 
    public float moveSpeed = 2f;
    public float pauseDuration = 1f;

    private Vector3 initialPosition;
    private bool movingToEnd = true;

    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(MoveLog());
    }

    IEnumerator MoveLog()
    {
        while (true)
        {
            //calculate target position based on current movement direction
            Vector3 targetPosition = movingToEnd ? endPoint.position : startPoint.position;

            //move the log towards target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            //check if log has reached target position
            if (transform.position == targetPosition)
            {
                //toggle movement direction
                movingToEnd = !movingToEnd;

                //pause for a specified duration at each end point
                yield return new WaitForSeconds(pauseDuration);
            }

            yield return null;
        }
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
