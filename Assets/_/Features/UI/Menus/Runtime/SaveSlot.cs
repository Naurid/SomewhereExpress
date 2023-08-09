using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
   [Header("Profile")]
   [SerializeField] private string profileId = "";

   [Header("Content")]
   [SerializeField] private GameObject noDataContent;
   [SerializeField] private GameObject hasDataContent;
   [SerializeField] private TMP_Text text;

   private Button saveSlotButton;

   private void Awake() 
   {
      saveSlotButton = this.GetComponent<Button>();
   }

   public void SetData(SaveData data) 
   {
      if (data == null) 
      {
         noDataContent.SetActive(true);
         hasDataContent.SetActive(false);
      }
      else 
      {
         noDataContent.SetActive(false);
         hasDataContent.SetActive(true);

         text.text = $"We are at the scene {data.m_sceneIndex}";
      }
   }

   public string GetProfileId() 
   {
      return this.profileId;
   }

   public void SetInteractable(bool interactable)
   {
      saveSlotButton.interactable = interactable;
   }
}
