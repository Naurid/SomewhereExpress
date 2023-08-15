using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuParent
{
    #region Unity API

    private void Start()
    {
        if (!DataPersistenceManager.m_instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }

    #endregion

    #region Main Methods

    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        // load the next scene - which will in turn load the game because of 
        // OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("SampleScene");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Private and protected

    [Header("Menu Navigation")] [SerializeField]
    private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")] [SerializeField]
    private Button newGameButton;

    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGameButton;

    #endregion
}