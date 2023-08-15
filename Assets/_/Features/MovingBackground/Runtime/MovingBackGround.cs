using System.Collections.Generic;
using UnityEngine;

public class MovingBackGround : MonoBehaviour
{
    #region Public Members

    [HideInInspector] public List<GameObject> m_decor = new();

    #endregion


    #region Unity API

    private void Start()
    {
        _anchors[1].forward = -_anchors[0].forward;
        if (m_decor.Count < _numberOfDecorAtOnce)
        {
            foreach (var anchor in _anchors)
            {
                SpawnNewDecor(anchor);
            }
        }
    }

    #endregion


    #region Main Methods

    public void SpawnNewDecor(Transform parent)
    {
        if (m_decor.Count >= _numberOfDecorAtOnce) return;
        
        GameObject newDecor = Instantiate(_decorPrefab, parent.position, parent.rotation, parent);
        MovingDecor decorMover = newDecor.GetComponent<MovingDecor>();
        AssignData(parent, decorMover);
        m_decor.Add(newDecor);

    }

    private void AssignData(Transform parent, MovingDecor decorMover)
    {
        decorMover.m_maxDistance = _maxDistance;
        decorMover.m_frequency = _spawnFrequency;
        decorMover.m_movementDirection = _anchors[0].forward;
        decorMover.m_treePrefab = _treePrefab;
        decorMover.m_manager = this;
        decorMover.m_speed = _speed;
        decorMover.m_parent = parent;
    }

    #endregion
    
    
    #region Private and protected

    [Header("Objects")]
    [SerializeField] private Transform[] _anchors = new Transform[2];
    [SerializeField] private GameObject _decorPrefab;
    [SerializeField] private GameObject _treePrefab;
    
    [Space]
    [Header("Data")]
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _spawnFrequency;
    [SerializeField] private float _speed;
    [SerializeField] private int _numberOfDecorAtOnce;

    #endregion
    
}
