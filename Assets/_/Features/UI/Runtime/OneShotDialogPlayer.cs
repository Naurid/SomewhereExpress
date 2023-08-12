using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneShotDialogPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _dialogObject;
    [SerializeField] private bool _isInstant;
    [SerializeField] private Dialog _dialog;
    [SerializeField] private float _typeSpeed;
    private List<char> letterList = new();
    private bool _isDialogDone;

    [SerializeField] private InputAction _skip;

    private void OnEnable()
    {
        _skip.started += SkipDialog;
        _skip.Enable();
    }

    private void SkipDialog(InputAction.CallbackContext ctx)
    {
        if (!_isDialogDone)
        {
            StopAllCoroutines();
            _dialogObject.transform.GetChild(2).GetComponent<TMP_Text>().text = _dialog.m_message;
            _isDialogDone = true;
        }
        else if(_isDialogDone)
        {
           
            _dialogObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _skip.started -= SkipDialog;
        _skip.Disable();
    }

    public void PlayDialog()
    {
        _dialogObject.SetActive(true);
        _dialogObject.transform.GetChild(1).GetComponent<TMP_Text>().text = _dialog.m_name;

        if (_isInstant)
        {
            _dialogObject.transform.GetChild(2).GetComponent<TMP_Text>().text = _dialog.m_message;
            _isDialogDone = true;
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
        else if(letterList.Count == _dialog.m_message.Length)
        {
            _isDialogDone = true;
        }
    }
}
