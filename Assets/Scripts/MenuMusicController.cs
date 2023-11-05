using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMusicController : MonoBehaviour
{
    public AudioSource intro;
    public AudioSource click;
    public bool isOn = true;

    public void Start()
    {
        if(!isOn)
        {
        intro.Pause();
        }
        else
        {
         intro.Play();
        }
    }
    public void PlayMusic()
    {
        PlayClickSound();
        if(isOn)
        {
            //do nothing coz because the music is already playing.
        }
        else
        {
          intro.Play();
          isOn = true;
        }
        
    }
    public void StopMusic()
    {
        PlayClickSound();
        if(isOn)
        {
          intro.Pause();
          isOn = false;
        }
        else
        {
          //do nothing because the music was aready off.
        }
        
    }
     
     public void PlayClickSound()
     {
        click.Play();
     }
}
