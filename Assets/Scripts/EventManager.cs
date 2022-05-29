using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Current;

    private void Awake()
    {
        if (Current == null)
        {
            Current = FindObjectOfType<EventManager>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
    
    public event Action PlayerDeath;

    public void PlayerDead()
    {
        if (PlayerDeath != null)
        {
            PlayerDeath();
        }
    }
    
    public event Action PlayerSpawn;

    public void PlayerSpawnIn()
    {
        if (PlayerSpawn != null)
        {
            PlayerSpawn();
        }
    }

    /*public event Action StartTheDayEvent;

    public void StartTheDay()
    {
        if (StartTheDayEvent != null)
        {
            StartTheDayEvent();
        }
    }*/
}
