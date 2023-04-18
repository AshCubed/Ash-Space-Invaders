using UnityEngine;

public class GunDamagePowerUp : PowerUpBase
{
    [SerializeField] private int _newPlayerGunDamageAmnt;
    [SerializeField] private GameObject _activeDamageText;
    private int _oldPlayerGunDamageAmnt;
    private PlayerController _playerController;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _activeDamageText.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        _playerController = FindObjectOfType<PlayerController>();
        _oldPlayerGunDamageAmnt = _playerController.BulletDamageAmount;
        _playerController.BulletDamageAmount = _newPlayerGunDamageAmnt;
    }

    protected override void EndPowerUp()
    {
        _playerController.BulletDamageAmount = _oldPlayerGunDamageAmnt;
        Destroy(gameObject);
    }
}
