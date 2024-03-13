using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public float openSpeed = 2f;
    public Vector3 openDirection = Vector3.left;

    private bool isOpen = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        //door start position
        initialPosition = transform.position;
        targetPosition = initialPosition + openDirection;
    }

    public void OpenDoor()
    {
        //check if the door is already open
        if (!isOpen)
        {
            isOpen = true;
            //add sfx here

            //moove door to target position
            StartCoroutine(MoveDoor(targetPosition));
        }
    }

    private IEnumerator MoveDoor(Vector3 target)
    {
        while (transform.position != target)
        {
            //move door towards target position
            transform.position = Vector3.MoveTowards(transform.position, target, openSpeed * Time.deltaTime);

            //check if door is at target position
            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                //snap door to target position
                transform.position = target;

                //exit
                yield break;
            }
            yield return null;
        }
    }
}
