using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{

    //clicky noise make brain go bbrrrrrr

    public AudioSource buttonClicks;
    public AudioClip[] buttonClicksArray;

    public void PlayButtonClick()
    {
        //make it random so they can't predict your next move
        int randomIndex = Random.Range(0, buttonClicksArray.Length);
        buttonClicks.clip = buttonClicksArray[randomIndex];
        buttonClicks.Play();
    }
}
