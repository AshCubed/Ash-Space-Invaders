using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    public float moveBy;
    public GameObject bullet;
    public int bulletDmgAmnt;
    public Transform bulletSpawnPoint;
    public ParticleSystem deathEffect;

    public int lives = 3;
    public Transform livesImgParent;
    public GameObject livesImg;
    public Transform livesSpwnPnt;
    private List<GameObject> _livesImgs;
    
    private Vector3 _startingPos;
    public PlayableDirector gameOverDirector;
    public GameObject playerShield;

    private bool _canPlayerMove;

    // Start is called before the first frame update
    void Start()
    {
        _canPlayerMove = false;
        _startingPos = transform.position;
        _livesImgs = new List<GameObject>();
        for (int i = 0; i < lives; i++)
        {
            GameObject newGo = Instantiate(livesImg, new Vector2(livesSpwnPnt.transform.position.x + (100 * i),
                livesSpwnPnt.transform.position.y), Quaternion.identity, livesImgParent);
            _livesImgs.Add(newGo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Controls();
    }

    private void Controls()
    {
        if (_canPlayerMove)
        {
            if (Input.GetKey(KeyCode.D) && transform.position.x < 7.5f)
            {
                transform.position += new Vector3(moveBy, 0, 0);
            }
            else if (Input.GetKey(KeyCode.A) && transform.position.x > -7.5f)
            {
                transform.position += new Vector3(-moveBy, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                FireBullet();
            }
        }
    }

    private void FireBullet()
    {
        AudioManager.instance.Play("ShipBullet");
        GameObject newGo = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        newGo.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 40);
        newGo.GetComponent<BulletLogic>().bulletDmgAmnt = bulletDmgAmnt;
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
            Instantiate(playerShield, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        }

        if (other.gameObject.CompareTag("PowerUp-GunDmgIncr"))
        {
            other.gameObject.GetComponent<GunDamagePowerUp>().enabled = true;
        }
    }
    

    IEnumerator Death()
    {
        GetComponent<Collider2D>().enabled = false;
        FindObjectOfType<InvaderGroupBrain>().CanInvadersFire(false);
        AudioManager.instance.Play("InvaderHit");
        Instantiate(deathEffect.gameObject, transform.position, deathEffect.transform.rotation);
        _canPlayerMove = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        lives--;
        if (lives != 0)
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
            gameOverDirector.Play();
        }
    }

    public void CanPlayerMove(bool x)
    {
        _canPlayerMove = x;
    }
}
