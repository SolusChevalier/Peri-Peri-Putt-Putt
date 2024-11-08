//using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject endPanel;

    #endregion

    #region UNITY METHODS

    private void Update()
    {
        if (!settingsPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            ShowSettings();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideSettings();
        }

    }
    #endregion



    #region METHODS
    public void PlayButton()
    {
        SceneManager.LoadScene(2);
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowEnding() 
    {
        endPanel.SetActive(true);
    }

    public void HideEnding() 
    {
        endPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LoadFacebook() 
    {
        Application.OpenURL("https://www.facebook.com/");
    }

    public void LoadX() 
    {
        Application.OpenURL("https://twitter.com/home");
    }

    public void LoadInstagram() 
    {
        Application.OpenURL("https://www.instagram.com/");
    }

    public void LoadNandos() 
    {
        Application.OpenURL("https://www.nandos.co.za/");
    }
    #endregion
}