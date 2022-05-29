using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int scoreToAddOnDeath;
    public GameObject enemyBullet;
    public Transform bulletSpawnPoint;
    public ParticleSystem invaderDeathEffect;

    public bool canFire;
    public Enemy nextToFire;
    
    public enum EnemyLevel {Normal, Lvl1, Lvl2};
    public EnemyLevel currentEnemyLevel;

    private void Start()
    {
        health = maxHealth;
        currentEnemyLevel = EnemyLevel.Normal;
    }

    public virtual void SpawnBullet()
    {
        AudioManager.instance.Play("InvaderBullet");
        GameObject newGo = Instantiate(enemyBullet, bulletSpawnPoint.position, Quaternion.identity);
        newGo.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 40);
    }

    public virtual void OnBulletHit(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health -= other.gameObject.GetComponent<BulletLogic>().bulletDmgAmnt;
            if (health >= maxHealth && health != 0)
            {
                Destroy(other.gameObject);
                GameObject particleDeath = Instantiate(invaderDeathEffect.gameObject, transform.position, 
                    invaderDeathEffect.transform.rotation);
                ParticleSystem particleSystem = particleDeath.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = GetComponent<SpriteRenderer>().color;
            }
            else
            {
                AudioManager.instance.Play("InvaderHit");
                GameObject particleDeath = Instantiate(invaderDeathEffect.gameObject, transform.position, 
                    invaderDeathEffect.transform.rotation);
                ParticleSystem particleSystem = particleDeath.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = GetComponent<SpriteRenderer>().color;
                Destroy(other.gameObject);
                if (nextToFire != null)
                    nextToFire.canFire = true;
                Debug.Log("+" + scoreToAddOnDeath.ToString());
                UiManager.Instance.UpdateScore(scoreToAddOnDeath);
                InvaderGroupBrain.Instance.InvaderHasDied();
                Destroy(this.gameObject);
                
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnBulletHit(other);
    }
}
