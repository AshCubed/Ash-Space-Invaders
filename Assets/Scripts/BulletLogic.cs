using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public int bulletDmgAmnt;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DESTROY"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("InvaderBullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
