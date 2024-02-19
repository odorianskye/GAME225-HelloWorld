using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegHandler : MonoBehaviour
{
    private GameObject[] discs;
    private int numDiscs;

    void Start()
    {
        discs = new GameObject[4];
        numDiscs = 0;
    }

    void Update()
    {

    }

    //check if the disc can be placed on this peg
    public bool CanPlaceDisc(GameObject disc)
    {
        if (numDiscs == 0)
            return true;

        GameObject topDisc = discs[numDiscs - 1];
        return disc.transform.localScale.x < topDisc.transform.localScale.x;
    }

    //place the disc on this peg
    public Vector3 PlaceDisc(GameObject disc)
    {
        discs[numDiscs] = disc;
        numDiscs++;

        Vector3 newPosition = transform.position;
        //adjust the y position based on the number of discs
        newPosition.y += numDiscs * 0.5f; 
        return newPosition;
    }
}
