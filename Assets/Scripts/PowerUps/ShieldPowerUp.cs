using UnityEngine;

public class ShieldPowerUp : PowerUpBase
{
    [SerializeField] private Animator _shieldAnimator;

    protected override void EndPowerUp()
    {
        _shieldAnimator.SetTrigger("EndShield");
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
