using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _isPaused;
    [SerializeField] private GameObject _pausePanel;
    private void Update()
    {
        if (_isPaused)
        {
            Time.timeScale = 0f;
            _pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            _pausePanel.SetActive(false);
        }
    }

    public void Resume()
    {
        _isPaused = false;
    }

    public void GoToMainMenu()
    {
        _isPaused = false;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
