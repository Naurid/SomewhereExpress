using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuParent
{
    private bool _isPaused;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _optionsPanel;

    public void PauseGame()
    {
        if (_isPaused) return;
       
        Time.timeScale = 0f;
        _pausePanel.SetActive(true);
        _isPaused = true;
    }
    
    public void Resume()
    {
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
        _isPaused = false;
    }

    public void Options()
    {
        _optionsPanel.SetActive(true);
        _pausePanel.SetActive(false);
    }

    public void SaveGame()
    {
        DataPersistenceManager.instance.SaveGame();
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
