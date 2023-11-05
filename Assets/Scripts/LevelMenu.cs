using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public GameObject mainMenuInLevelsScript;
    public MenuMusicController menuClick;
  // public AdsInitializer ads;
    public void GoToMainMenuFromLevelsMenu()
    {
        menuClick.PlayClickSound();
        //ads.LoadInerstitialAd();
        gameObject.SetActive(false);
        mainMenuInLevelsScript.SetActive(true);
    }
}
