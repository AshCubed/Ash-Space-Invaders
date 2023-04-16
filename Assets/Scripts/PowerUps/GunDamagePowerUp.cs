using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GunDamagePowerUp : PowerUpBase
{
    public static GunDamagePowerUp Instance;
    
    public int newPlayerGunDamageAmnt;
    public int oldPlayerGunDamageAmnt;
    private PlayerController _playerController;
    public GameObject activeDamageText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        activeDamageText.SetActive(true);
        startTime = Time.fixedTime;
        endTime = Time.fixedTime + activeTime;
        endPowerUp = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        _playerController = FindObjectOfType<PlayerController>();
        oldPlayerGunDamageAmnt = _playerController.BulletDamageAmount;
        _playerController.BulletDamageAmount = newPlayerGunDamageAmnt;
    }

    // Update is called once per frame
    void Update()
    {
        base.PowerUpTimerCheck();
    }
    
    public override void EndPowerUp()
    {
        _playerController.BulletDamageAmount = oldPlayerGunDamageAmnt;
        Destroy(gameObject);
    }
}
