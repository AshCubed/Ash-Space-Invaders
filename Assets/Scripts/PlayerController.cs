using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveBy = 0.05f;
    [SerializeField] private float _screenLeftMax, _screenRightMax;
    [SerializeField] private InputActionReference _actionMovement;
    [SerializeField] private InputActionReference _actionFireWeapon;
    [Header("Gun")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private int _bulletDmgAmnt = 5;
    [SerializeField] private Transform _bulletSpawnPoint;
    [Header("Life")]
    [SerializeField] private ParticleSystem _deathEffect;
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _playerShield;
    [Header("Ui")]
    [SerializeField] private Transform _livesImgParent;
    [SerializeField] private GameObject _livesImg;
    [SerializeField] private Transform _livesSpwnPnt;
    [SerializeField] private PlayableDirector _gameOverDirector;

    private Vector2 _movementDirection;
    private List<GameObject> _livesImgs;
    private Vector3 _startingPos;
    private bool _canPlayerMove;
    
    public int BulletDamageAmount
    {
        get => _bulletDmgAmnt;
        set => _bulletDmgAmnt = value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _canPlayerMove = true;
        _startingPos = transform.position;
        _livesImgs = new List<GameObject>();
        for (int i = 0; i < _lives; i++)
        {
            GameObject newGo = Instantiate(_livesImg, new Vector2(_livesSpwnPnt.transform.position.x + (100 * i),
                _livesSpwnPnt.transform.position.y), Quaternion.identity, _livesImgParent);
            _livesImgs.Add(newGo);
        }
    }

    private void Update()
    {
        if (_canPlayerMove)
        {
            if (_movementDirection.x >= 0.8f && transform.position.x < _screenRightMax)
            {
                transform.position += new Vector3(_moveBy, 0, 0);
            }
            else if (_movementDirection.x <= -0.8f && transform.position.x > _screenLeftMax)
            {
                transform.position += new Vector3(-_moveBy, 0, 0);
            }
        }
        
    }

    private void FireBullet()
    {
        AudioManager.instance.Play("ShipBullet");
        GameObject newGo = Instantiate(_bullet, _bulletSpawnPoint.position, Quaternion.identity);
        newGo.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 40);
        newGo.GetComponent<BulletLogic>().bulletDmgAmnt = _bulletDmgAmnt;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("InvaderBullet"))
        {
            Destroy(other.gameObject);
            StartCoroutine(Death());
        }

        if (other.gameObject.CompareTag("PowerUp-Shield"))
        {
            Destroy(other.gameObject);
            Instantiate(_playerShield, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        }

        if (other.gameObject.CompareTag("PowerUp-GunDmgIncr"))
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<GunDamagePowerUp>().enabled = true;
        }
    }
    

    private IEnumerator Death()
    {
        GetComponent<Collider2D>().enabled = false;
        FindObjectOfType<InvaderGroupBrain>().CanInvadersFire(false);
        AudioManager.instance.Play("InvaderHit");
        Instantiate(_deathEffect.gameObject, transform.position, _deathEffect.transform.rotation);
        _canPlayerMove = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        _lives--;
        if (_lives != 0)
        {
            GetComponent<Collider2D>().enabled = true;
            transform.position = _startingPos;
            Destroy(_livesImgs[_livesImgs.Count-1]);
            _livesImgs.Remove(_livesImgs[_livesImgs.Count-1]);
            this.GetComponent<SpriteRenderer>().enabled = true;
            _canPlayerMove = true;
            yield return new WaitForSeconds(0.5f);
            FindObjectOfType<InvaderGroupBrain>().CanInvadersFire(true);
        }
        else
        {
            Destroy(_livesImgs[_livesImgs.Count-1]);
            yield return new WaitForSeconds(0.5f);
            UiManager.Instance.SetFinalScoreScreen();
            _gameOverDirector.Play();
        }
    }

    public void CanPlayerMove(bool x)
    {
        _canPlayerMove = x;
    }

    private void OnEnable()
    {
        _actionMovement.action.Enable();
        _actionFireWeapon.action.Enable();
        _actionMovement.action.performed += ActionMovementOnPerformed;
        _actionMovement.action.canceled += ActionMovementOnCanceled;
        _actionFireWeapon.action.started += ActionFireWeaponOnStarted;
    }

    private void ActionMovementOnCanceled(InputAction.CallbackContext obj)
    {
        _movementDirection = Vector2.zero;
    }

    private void ActionFireWeaponOnStarted(InputAction.CallbackContext obj)
    {
        FireBullet();
    }

    private void ActionMovementOnPerformed(InputAction.CallbackContext obj)
    {
        if (_canPlayerMove)
        {
            _movementDirection = new Vector2(obj.ReadValue<Vector2>().x, 0f);
        }
    }

    private void OnDisable()
    {
        _actionMovement.action.Disable();
        _actionFireWeapon.action.Disable();
        _actionMovement.action.performed -= ActionMovementOnPerformed;
        _actionMovement.action.canceled -= ActionMovementOnCanceled;
        _actionFireWeapon.action.started -= ActionFireWeaponOnStarted;
    }
}
