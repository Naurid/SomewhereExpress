using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
   #region Unity API

   private void Awake() 
   {
      _saveSlotButton = this.GetComponent<Button>();
   }

   #endregion

   #region Main Methods

   public void SetData(SaveData data) 
   {
      if (data == null) 
      {
         _noDataContent.SetActive(true);
         _hasDataContent.SetActive(false);
      }
      else 
      {
         _noDataContent.SetActive(false);
         _hasDataContent.SetActive(true);

         _text.text = $"We are at the scene {data.m_sceneIndex}";
      }
   }

   public string GetProfileId() 
   {
      return this._profileId;
   }

   public void SetInteractable(bool interactable)
   {
      _saveSlotButton.interactable = interactable;
   }

   #endregion

   #region Private and protected

   [Header("Profile")]
   [SerializeField] private string _profileId = "";

   [Header("Content")]
   [SerializeField] private GameObject _noDataContent;
   [SerializeField] private GameObject _hasDataContent;
   [SerializeField] private TMP_Text _text;

   private Button _saveSlotButton;

   #endregion
}