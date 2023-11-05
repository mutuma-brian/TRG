using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;


public class CanvasScript: MonoBehaviour
{
    public GameObject pausedTextMesh;
    public AudioSource clickAudio;
    public CinemachineVirtualCamera defaultCamera;
    public CinemachineVirtualCamera frontCamera;
    public CinemachineVirtualCamera reverseCamera;
    public AdsInitializer adss;
    public CinemachineVirtualCamera[] virtualCameras;

    private int zeroTimeScale = 0;
    private int oneTimeScale = 1;
    public int cameraCount = 0;

  void Start()
    {
       pausedTextMesh.SetActive(false);
       virtualCameras = new CinemachineVirtualCamera[] { defaultCamera, frontCamera, reverseCamera};
       defaultCamera.MoveToTopOfPrioritySubqueue();  
    }

   
    public void PauseGame()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            clickAudio.Play();
        if(Time.timeScale == oneTimeScale)
        {
            audioSource.Pause();
            Time.timeScale = zeroTimeScale;
            pausedTextMesh.SetActive(true);
         }
        else if (Time.timeScale == zeroTimeScale )
        {
            audioSource.UnPause();
            pausedTextMesh.SetActive(false);
            Time.timeScale = oneTimeScale;
            adss.LoadInerstitialAd();
        }
        }
         

    }

    public void BackToMenu()
    {
       clickAudio.Play();
       SceneManager.LoadScene("MenuScene");
       
    }

    public void SwitchVirtualCamera()
    {
        if(cameraCount<2)
        {
        clickAudio.Play();
        cameraCount++;
        virtualCameras[cameraCount].MoveToTopOfPrioritySubqueue();  
        }
        else 
        {
         clickAudio.Play();
         cameraCount=0;
         virtualCameras[cameraCount].MoveToTopOfPrioritySubqueue();  
        }  
        
    }

    public void RestartGame()
    {
        clickAudio.Play();
        SceneManager.LoadScene("PlayScene");
         if(Time.timeScale== zeroTimeScale)
         {
            pausedTextMesh.SetActive(true);
         }
    }


}
