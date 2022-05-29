using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUpBase
{
    public Animator shiledAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.fixedTime;
        endTime = Time.fixedTime + activeTime;
        endPowerUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        base.PowerUpTimerCheck();
    }

    public override void EndPowerUp()
    {
        shiledAnimator.SetTrigger("EndShield");
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
