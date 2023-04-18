using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int scoreToAddOnDeath;
    public GameObject enemyBullet;
    public Transform[] bulletSpawnPoint;
    public ParticleSystem invaderDeathEffect;
    [SerializeField] private float _bulletForce = 40f;

    public bool canFire;
    public Enemy nextToFire;

    private void Start()
    {
        health = maxHealth;
    }

    public void SpawnBullet()
    {
        foreach (var item in bulletSpawnPoint)
        {
            AudioManager.instance.Play("InvaderBullet");
            var newGo = Instantiate(enemyBullet, item.position, Quaternion.identity);
            newGo.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * _bulletForce);
        }
    }

    private void OnBulletHit(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health -= other.gameObject.GetComponent<BulletLogic>().bulletDmgAmnt;
            if (health >= maxHealth && health != 0)
            {
                Destroy(other.gameObject);
                var particleDeath = Instantiate(invaderDeathEffect.gameObject, transform.position, 
                    invaderDeathEffect.transform.rotation);
                var particleSystem = particleDeath.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = GetComponent<SpriteRenderer>().color;
            }
            else
            {
                AudioManager.instance.Play("InvaderHit");
                var particleDeath = Instantiate(invaderDeathEffect.gameObject, transform.position, 
                    invaderDeathEffect.transform.rotation);
                var particleSystem = particleDeath.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = GetComponent<SpriteRenderer>().color;
                Destroy(other.gameObject);
                if (nextToFire != null)
                    nextToFire.canFire = true;
                UiManager.Instance.UpdateScore(scoreToAddOnDeath);
                InvaderGroupBrain.Instance.InvaderHasDied();
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnBulletHit(other);
    }
}
