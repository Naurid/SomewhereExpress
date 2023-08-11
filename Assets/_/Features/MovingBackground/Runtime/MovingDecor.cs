
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovingDecor : MonoBehaviour
{
    public Transform m_parent;
    public float m_maxDistance;
    public float m_frequency;
    public Vector3 m_movementDirection;
    public float m_speed;
    public GameObject m_treePrefab;
    [SerializeField] private List<Transform> _treeSpawnPoints = new();

    public MovingBackGround m_manager;

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
            m_manager._decor.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(m_frequency);
        m_manager.SpawnNewDecor(m_parent);
    }
}
