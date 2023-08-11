using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackGround : MonoBehaviour
{
    [SerializeField] private Transform[] _wagon = new Transform[2];
    [SerializeField] private GameObject _decorPrefab;
    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _spawnFrequency;
    [SerializeField] private float _speed;
    [SerializeField] private int _numberOfDecorAtOnce;

    public List<GameObject> _decor = new();

    private void Start()
    {
        _wagon[1].forward = -_wagon[0].forward;
        if (_decor.Count < _numberOfDecorAtOnce)
        {
            foreach (var anchor in _wagon)
            {
                SpawnNewDecor(anchor);
            }
        }
    }

    public void SpawnNewDecor(Transform parent)
    {
        if (_decor.Count >= _numberOfDecorAtOnce) return;
        
        GameObject newDecor = Instantiate(_decorPrefab, parent.position, parent.rotation, parent);
        MovingDecor decorMover = newDecor.GetComponent<MovingDecor>();
        decorMover.m_maxDistance = _maxDistance;
        decorMover.m_frequency = _spawnFrequency;
        decorMover.m_movementDirection = _wagon[0].forward;
        decorMover.m_treePrefab = _treePrefab;
        decorMover.m_manager = this;
        decorMover.m_speed = _speed;
        decorMover.m_parent = parent;
        _decor.Add(newDecor);

    }
}
