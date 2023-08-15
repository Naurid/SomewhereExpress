using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDecor : MonoBehaviour
{
    #region Public Members

    [HideInInspector] public MovingBackGround m_manager;
    
    [HideInInspector] public Transform m_parent;
    [HideInInspector] public GameObject m_treePrefab;
    
    [HideInInspector] public Vector3 m_movementDirection;
    
    [HideInInspector] public float m_maxDistance;
    [HideInInspector] public float m_frequency;
    [HideInInspector] public float m_speed;

    #endregion


    #region Unity API

    private void Start()
    {
        foreach (var point in _treeSpawnPoints)
        {
            int randInt = Random.Range(0, 2);
            if (randInt == 0) continue;

            Instantiate(m_treePrefab, point);
        }
        
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        this.transform.position += m_movementDirection * (Time.deltaTime * m_speed);
        
        if (Vector3.Distance(m_parent.position, this.transform.position) > m_maxDistance)
        {
            m_manager.m_decor.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    #endregion

    
    #region Main Methods

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(m_frequency);
        m_manager.SpawnNewDecor(m_parent);
    }

    #endregion


    #region Private and protected

    [SerializeField] private List<Transform> _treeSpawnPoints = new();

    #endregion
}