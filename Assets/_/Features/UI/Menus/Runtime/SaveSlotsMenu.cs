using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MenuParent
{
    #region Unity API

    private void Awake()
    {
        _saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    #endregion

    #region Main Methods

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        // disable all buttons
        DisableMenuButtons();

        // update the selected profile id to be used for data persistence
        DataPersistenceManager.m_instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (!_isLoadingGame)
        {
            // create a new game - which will initialize our data to a clean slate
            DataPersistenceManager.m_instance.NewGame();
            DataPersistenceManager.m_instance.m_isLoaded = false;
            SceneManager.LoadSceneAsync(1);
        }

        // load the scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync(DataPersistenceManager.m_instance.GetData().m_sceneIndex);
    }

    public void OnBackClicked()
    {
        _mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        // set mode
        this._isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, SaveData> profilesGameData = DataPersistenceManager.m_instance.GetAllProfilesGameData();

        // loop through each save slot in the UI and set the content appropriately
        GameObject firstSelected = _backButton.gameObject;
        foreach (SaveSlot saveSlot in _saveSlots)
        {
            SaveData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(_backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        // set the first selected button
        StartCoroutine(this.SetFirstSelected(firstSelected));
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in _saveSlots)
        {
            saveSlot.SetInteractable(false);
        }

        _backButton.interactable = false;
    }

    #endregion

    #region Private and protected

    [Header("Menu Navigation")] [SerializeField]
    private MainMenu _mainMenu;

    [Header("Menu Buttons")] [SerializeField]
    private Button _backButton;

    private SaveSlot[] _saveSlots;

    private bool _isLoadingGame = false;

    #endregion
}