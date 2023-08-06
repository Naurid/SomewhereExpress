using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BasicFade : MonoBehaviour
{

    #region Unity API

    // private void OnGUI()
    // {
    //     if (GUILayout.Button("fade"))
    //     {
    //         SetBaseAlpha(_isFadeIn? 1f : 0f);
    //
    //         StartCoroutine(DoTheFade(_isFadeIn));
    //     }
    // }

    private void SetBaseAlpha(float baseAlpha)
    {
        _fadeOutColor = _fadeOutPanel.GetComponent<Image>().color;
        _fadeOutColor.a = baseAlpha;
        _fadeOutPanel.GetComponent<Image>().color = _fadeOutColor;
        _fadeOutPanel.SetActive(true);
    }

    #endregion
    
    
    #region Main Methods

    private IEnumerator DoTheFade(bool fadeIn)
    {
        yield return new WaitForSeconds(_fadeSpeed);
        
        _fadeOutColor = _fadeOutPanel.GetComponent<Image>().color;

        if (fadeIn)
        {
            if (_fadeOutPanel.GetComponent<Image>().color.a <= 0f)
            {
                StopAllCoroutines();
            }
            else
            {
                _fadeOutColor.a -= _fadeValue;
                _fadeOutPanel.GetComponent<Image>().color = _fadeOutColor;
                StartCoroutine(DoTheFade(_isFadeIn));
            }
        }
        else
        {
            if (_fadeOutPanel.GetComponent<Image>().color.a >= 1f)
            {
                StopAllCoroutines();
            }
            else
            {
                _fadeOutColor.a += _fadeValue;
                _fadeOutPanel.GetComponent<Image>().color = _fadeOutColor;
                StartCoroutine(DoTheFade(_isFadeIn));

            }
        }
       
    }

    #endregion
    
    
    #region Private and Protected

    [SerializeField] private bool _isFadeIn;
    [Space]
    [SerializeField] private GameObject _fadeOutPanel;
    [SerializeField] private float _fadeSpeed;
    [SerializeField][Range(0,1f)] private float _fadeValue;

    private Color _fadeOutColor;

    #endregion
}
