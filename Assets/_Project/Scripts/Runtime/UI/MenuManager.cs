//using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

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
        SceneManager.LoadScene(1);
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene(0);
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
    #endregion
}