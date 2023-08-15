using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuParent : MonoBehaviour
{
    #region Unity API

    protected void OnEnable()
    {
        StartCoroutine(SetFirstSelected(firstSelected));
    }

    #endregion

    #region Main Methods

    public IEnumerator SetFirstSelected(GameObject firstSelectedObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);
    }

    #endregion

    #region Private and protected

    [SerializeField] private GameObject firstSelected;

    #endregion
}