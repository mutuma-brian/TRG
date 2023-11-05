using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public GameObject settingsMenu; 
    public GameObject rulesMenu;
    public GameObject loadingMenu;
    public GameObject levelsMenu;
    public MenuMusicController menuClickSound;
   //public AdsInitializer adsss;

    public void PlayGame()
    {
      menuClickSound.PlayClickSound();
      GoToLoadingMenu();
    }

    public void Quit()
    {
        menuClickSound.PlayClickSound();
        Application.Quit();
    }

    public void GoToSettings()
    {
        menuClickSound.PlayClickSound();
        gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void GoToMenuFromSettings()
    {
        menuClickSound.PlayClickSound();
        settingsMenu.SetActive(false);
        gameObject.SetActive(true);
    }
    public void GoToMenuFromRules()
    {
        menuClickSound.PlayClickSound();
        rulesMenu.SetActive(false);
        gameObject.SetActive(true);
    }
    public void GoToRules()
    {
       menuClickSound.PlayClickSound();
       gameObject.SetActive(false);
      //adsss.LoadInerstitialAd();
       rulesMenu.SetActive(true);
    }
    public void GoToLoadingMenu()
    {
        menuClickSound.PlayClickSound();
        gameObject.SetActive(false);
        loadingMenu.SetActive(true);
        SceneManager.LoadScene("PlayScene");
    }
    public void GoToLevelsMenu()
    {
        menuClickSound.PlayClickSound();
        gameObject.SetActive(false);
        levelsMenu.SetActive(true);
    }



} 