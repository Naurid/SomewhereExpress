using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneShotDialogPlayer : MonoBehaviour
{
    #region Unity API

    private void OnEnable()
    {
        _skip.started += SkipDialog;
        _skip.Enable();
    }


    private void OnDisable()
    {
        _skip.started -= SkipDialog;
        _skip.Disable();
    }

    #endregion

    #region Main Methods

    private void SkipDialog(InputAction.CallbackContext ctx)
    {
        if (!_isDialogDone)
        {
            StopAllCoroutines();
            _dialogObject.transform.GetChild(2).GetComponent<TMP_Text>().text = _dialog[index].m_message;
            _isDialogDone = true;
        }
        else if (_isDialogDone)
        {
            index++;
            PlayDialog();
        }
    }

    public void PlayDialog()
    {
        if (_dialog[index] == null) return;

        _dialogObject.SetActive(true);
        _dialogObject.transform.GetChild(1).GetComponent<TMP_Text>().text = _dialog[index].m_name;

        if (_isInstant)
        {
            _dialogObject.transform.GetChild(2).GetComponent<TMP_Text>().text = _dialog[index].m_message;
            _isDialogDone = true;
        }
        else
        {
            letterList.Clear();
            StartCoroutine(TypeWrite());
        }
    }

    private IEnumerator TypeWrite()
    {
        letterList.Add(_dialog[index].m_message[letterList.Count]);
        _dialogObject.transform.GetChild(2).GetComponent<TMP_Text>().text = string.Join("", letterList);

        yield return new WaitForSeconds(1f / _typeSpeed);
        if (letterList.Count != _dialog[index].m_message.Length)
            StartCoroutine(TypeWrite());
        else if (letterList.Count == _dialog[index].m_message.Length) _isDialogDone = true;
    }

    #endregion

    #region Private and protected

    [SerializeField] private GameObject _dialogObject;
    [SerializeField] private InputAction _skip;

    [SerializeField] private bool _isInstant;
    [SerializeField] private float _typeSpeed;

    private readonly List<Dialog> _dialog = new();
    private readonly List<char> letterList = new();

    private int index;
    private bool _isDialogDone;

    #endregion
}