using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuParent : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;

    protected void OnEnable()
    {
        StartCoroutine(SetFirstSelected(firstSelected));
    }

    public IEnumerator SetFirstSelected(GameObject firstSelectedObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);
    }
}
