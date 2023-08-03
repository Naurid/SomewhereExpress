using System;
using TMPro;
using UnityEngine;

public class DialogInstant : MonoBehaviour
{
    #region Unity API

    private void OnGUI()
    {
        if (GUILayout.Button("trigger dialog"))
        {
            _dialogObject.SetActive(true);
            TriggerDialog();
        }
    }

    #endregion


    #region Main Methods

    private void TriggerDialog()
    {
        _dialogTitle.text = _dialogAuthor;
        _dialogContent.text = _dialogMessage;
    }

    #endregion
    #region Private and Protected

    [SerializeField] private GameObject _dialogObject;
    [SerializeField] private TMP_Text _dialogTitle;
    [SerializeField] private TMP_Text _dialogContent;

    [Space]
    [SerializeField] private string _dialogAuthor;
    [SerializeField] private string _dialogMessage;

    #endregion
}
