using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private int _timeBetweenPowerUps;
    [SerializeField] private List<GameObject> _powerUps;
    [SerializeField] private GameObject _powerUpParent;
    private List<Transform> _powerUpSpawnLocations;
    private float _currentTime;
    private bool _canSpawn;

    // Start is called before the first frame update
    private void Start()
    {
        _canSpawn = true;
        _powerUpSpawnLocations = new List<Transform>();
        foreach (var child in _powerUpParent.transform.GetComponentsInChildren<Transform>()) 
            _powerUpSpawnLocations.Add(child);
        _powerUpSpawnLocations.Remove(_powerUpSpawnLocations[0]);
        _currentTime = _timeBetweenPowerUps;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_canSpawn)
        {
            if (_currentTime > 0)
            {
                _currentTime -= 0.01f;
                if (_currentTime <= 0)
                {
                    Instantiate(_powerUps[Random.Range(0, _powerUps.Count)],
                        _powerUpSpawnLocations[Random.Range(0, _powerUpSpawnLocations.Count)].position, 
                        Quaternion.identity, _powerUpParent.transform);
                    _currentTime = _timeBetweenPowerUps;
                }
            }
        }
    }
}
