using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBulletEnemy : Enemy
{
    void Start()
    {
        health = maxHealth;
    }

    public override void SpawnBullet()
    {
        AudioManager.instance.Play("InvaderBullet");
        GameObject newGo = Instantiate(enemyBullet, bulletSpawnPoint.position, Quaternion.identity);
        newGo.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 150f);
    }

    public override void OnBulletHit(Collision2D other)
    {
        base.OnBulletHit(other);
    }
}
