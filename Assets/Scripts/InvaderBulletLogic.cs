using UnityEngine;

public class InvaderBulletLogic : MonoBehaviour
{
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

        if (other.gameObject.CompareTag("playerShield") 
            || other.gameObject.CompareTag("PowerUp-Shield") 
            || other.gameObject.CompareTag("PowerUp-GunDmgIncr"))
        {
            Destroy(gameObject);
        }
    }
}
