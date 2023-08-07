using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler m_instance;
    public int m_currentSceneIndex;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this);
        }

        m_currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadScene(SaveData data)
    {
        SceneManager.LoadScene(data.m_sceneIndex);
    }
}
