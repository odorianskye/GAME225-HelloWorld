using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{

    //clicky noise make brain go bbrrrrrr

    public AudioSource buttonClicks;
    public AudioSource cannonFire;
    public AudioSource missedSplash;
    public AudioClip[] buttonClicksArray;
    public AudioClip[] cannonFireArray;
    public AudioClip[] missedSplashArray;


    public void PlayButtonClick()
    {
        //make it random so they can't predict your next move
        int randomIndex = Random.Range(0, buttonClicksArray.Length);
        buttonClicks.clip = buttonClicksArray[randomIndex];
        buttonClicks.Play();
    }

    public void PlayCannonFire()
    {

        cannonFire.Play();

    }

    public void PlaySplash()
    {

        missedSplash.Play();

    }


}
