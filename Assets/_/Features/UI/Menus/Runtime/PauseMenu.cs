using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuParent
{
    #region Unity API

    private void Start()
    {
        _isPaused = false;
    }

    private void Update()
    {
        if (!_isPaused)
            Resume();
        else
            PauseGame();
    }

    #endregion

    #region Main Methods

    public void ChangePauseState()
    {
        _isPaused = !_isPaused;
    }

    public void PauseGame()
    {
        //if (_isPaused) return;

        Time.timeScale = 0f;
        _pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
    }

    public void Options()
    {
        _optionsPanel.SetActive(true);
        _pausePanel.SetActive(false);
    }

    public void SaveGame()
    {
        DataPersistenceManager.m_instance.SaveGame();
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

    #endregion

    #region Private and protected

    private bool _isPaused;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _optionsPanel;

    #endregion
}