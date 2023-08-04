using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OneShotDialogPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _dialogObject;
    [SerializeField] private bool _isInstant;
    [SerializeField] private Dialog _dialog;
    [SerializeField] private float _typeSpeed;
    private List<char> letterList = new();
    
    public void PlayDialog()
    {
        _dialogObject.SetActive(true);
        _dialogObject.transform.GetChild(1).GetComponent<TMP_Text>().text = _dialog.m_name;

        if (_isInstant)
        {
            _dialogObject.transform.GetChild(2).GetComponent<TMP_Text>().text = _dialog.m_message;
        }
        else
        {
            letterList.Clear();
            StartCoroutine(TypeWrite());
        }
    }

    IEnumerator TypeWrite()
    {
        letterList.Add(_dialog.m_message[letterList.Count]);
        _dialogObject.transform.GetChild(2).GetComponent<TMP_Text>().text = String.Join("", letterList);
        
        yield return new WaitForSeconds(1f/_typeSpeed);
        if (letterList.Count != _dialog.m_message.Length)
        {
            StartCoroutine(TypeWrite());
        }
    }
}
