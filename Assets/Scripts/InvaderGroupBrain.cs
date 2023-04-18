using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class InvaderGroupBrain : MonoBehaviour
{
    public static InvaderGroupBrain Instance;

    [SerializeField] private List<GameObject> invaders;
    [SerializeField] private GameObject invaderParent;
    [SerializeField] private Transform spawnA;
    [SerializeField] private Transform spawnB;
    [SerializeField] private Transform spawnC;

    private bool _canStartFiring;
    private enum CurrentInvaderLocation {Normal, Left, Right, Down};
    private CurrentInvaderLocation _currentInvaderLocation;

    private int _totalInvaders;
    private int _deadInvaders;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        _currentInvaderLocation = CurrentInvaderLocation.Normal;
        _canStartFiring = false;
        StartCoroutine(SpawnInvaders());
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_canStartFiring) return;
        var rnd = Random.Range(0, 100);
        if (rnd is > 50 and < 52)
        {
            InvaderFire();
        }
    }

    private void InvaderFire()
    {
        var meme = invaderParent.GetComponentsInChildren<Enemy>();
        var meme2 = meme.ToList();
        var possibleEnemies = meme2.FindAll(x => x.canFire == true);
        var rnd = Random.Range(0, possibleEnemies.Count);
        if (possibleEnemies[rnd] != null)
            possibleEnemies[rnd].SpawnBullet();
    }

    IEnumerator SpawnInvaders()
    {
        for (int i = 0; i < 10; i++)
        {
            var newGo = Instantiate(invaders[Random.Range(0, invaders.Count-1)], 
                new Vector2(spawnA.position.x + (1.6f * i), spawnA.position.y), 
                Quaternion.identity, invaderParent.transform);
            var enemyA = newGo.GetComponent<Enemy>();
            enemyA.canFire = false;
            yield return new WaitForSeconds(0.1f);
            _totalInvaders++;
            
            var newGo2 = Instantiate(invaders[Random.Range(0, invaders.Count-1)], 
                new Vector2(spawnB.position.x + (1.6f * i), spawnB.position.y), 
                Quaternion.identity, invaderParent.transform);
            var enemyB = newGo2.GetComponent<Enemy>();
            enemyB.canFire = false;
            enemyB.nextToFire = enemyA;
            yield return new WaitForSeconds(0.1f);
            _totalInvaders++;
            
            var newGo3 = Instantiate(invaders[Random.Range(0, invaders.Count-1)], 
                new Vector2(spawnC.position.x + (1.6f * i), spawnC.position.y), 
                Quaternion.identity, invaderParent.transform);
            var enemyC = newGo3.GetComponent<Enemy>();
            enemyC.canFire = true;
            enemyC.nextToFire = enemyB;
            yield return new WaitForSeconds(0.1f);
            _totalInvaders++;
        }
        _canStartFiring = true;
        FindObjectOfType<PlayerController>().CanPlayerMove(true);
        StartCoroutine(MoveInvaders());
    }

    IEnumerator MoveInvaders()
    {
        //x +0.6 -0.6
        //y +0.5 - 0.5
        if (_currentInvaderLocation != CurrentInvaderLocation.Normal)
        {
            _currentInvaderLocation = CurrentInvaderLocation.Normal;
            invaderParent.transform.position = new Vector2(0, 0);
            yield return new WaitForSeconds(0.7f);
        }
        else
        {
            var rnd = Random.Range(0, 3);
            switch (rnd)
            {
                case 0:
                    _currentInvaderLocation = CurrentInvaderLocation.Left;
                    invaderParent.transform.position = new Vector2(invaderParent.transform.position.x - 0.6f,
                        invaderParent.transform.position.y);
                    break;
                case 1:
                    _currentInvaderLocation = CurrentInvaderLocation.Right;
                    invaderParent.transform.position = new Vector2(invaderParent.transform.position.x + 0.6f,
                        invaderParent.transform.position.y);
                    break;
                case 2:
                    _currentInvaderLocation = CurrentInvaderLocation.Down;
                    invaderParent.transform.position = new Vector2(invaderParent.transform.position.x,
                        invaderParent.transform.position.y - 0.5f);
                    break;
            }
            yield return new WaitForSeconds(0.7f);
        }

        if (_canStartFiring)
        {
            StartCoroutine(MoveInvaders());
        }
    }
    
    public void CanInvadersFire(bool x)
    {
        _canStartFiring = x;
        if (x)
            StartCoroutine(MoveInvaders());
        else
            StopCoroutine(MoveInvaders());
    }

    public void InvaderHasDied()
    {
        _deadInvaders++;
        if (_deadInvaders < _totalInvaders) return;
        _deadInvaders = 0;
        _totalInvaders = 0;
        FindObjectOfType<PlayerController>().CanPlayerMove(false);
        _canStartFiring = false;
        StartCoroutine(SpawnInvaders());
    }

}
