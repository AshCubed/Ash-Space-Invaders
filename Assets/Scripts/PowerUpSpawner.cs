using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public int timeBetweenPowerUps;
    public List<GameObject> powerUps;
    public List<Transform> powerUpSpawnLocations;
    public GameObject powerUpParent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(timeBetweenPowerUps);
        Instantiate(powerUps[Random.Range(0, powerUps.Count - 1)],
            powerUpSpawnLocations[Random.Range(0, powerUpSpawnLocations.Count - 1)].position, 
            Quaternion.identity, powerUpParent.transform);
        StartCoroutine(SpawnPowerUps());
    }
}
