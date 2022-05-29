using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeBulletEnemy : Enemy
{
    public Transform bulletSpawnPoint1;
    public Transform bulletSpawnPoint2;
    
    private void Start()
    {
        currentEnemyLevel = EnemyLevel.Lvl1;
        health = maxHealth;
        
    }

    public override void SpawnBullet()
    {
        base.SpawnBullet();
        
        AudioManager.instance.Play("InvaderBullet");
        GameObject newGo1 = Instantiate(enemyBullet, bulletSpawnPoint1.position, 
            new Quaternion(bulletSpawnPoint1.rotation.x, bulletSpawnPoint1.rotation.y, 
                    bulletSpawnPoint1.rotation.z, bulletSpawnPoint1.rotation.w));
        newGo1.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 30);
        
        AudioManager.instance.Play("InvaderBullet");
        GameObject newGo2 = Instantiate(enemyBullet, bulletSpawnPoint2.position,  
            new Quaternion(bulletSpawnPoint2.rotation.x, bulletSpawnPoint2.rotation.y, 
                bulletSpawnPoint2.rotation.z, bulletSpawnPoint2.rotation.w));
        newGo2.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 30);
    }

    public override void OnBulletHit(Collision2D other)
    {
        base.OnBulletHit(other);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnBulletHit(other);
    }
}
