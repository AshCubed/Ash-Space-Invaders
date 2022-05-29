using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    public int activeTime;
    public float startTime;
    public float endTime;
    public bool endPowerUp;
    
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
        PowerUpTimerCheck();
    }

    public virtual void PowerUpTimerCheck()
    {
        if (Time.fixedTime >= endTime && !endPowerUp)
        {
            EndPowerUp();
            endPowerUp = true;
        }
    }

    public virtual void EndPowerUp()
    {
        
    }
}
