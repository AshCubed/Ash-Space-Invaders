using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    
    public TextMeshProUGUI score;
    public TextMeshProUGUI endFinalScore;
    private int _currentScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _currentScore = 0;
        UpdateScore(_currentScore);
    }

    public void UpdateScore(int num)
    {
        _currentScore += num;
        score.text = "Score <" + _currentScore.ToString() + ">";
    }

    public void SetFinalScoreScreen()
    {
        int highScore = PlayerPrefs.GetInt("highScore");
        if (highScore != null)
        {
            if (_currentScore > highScore)
            {
                PlayerPrefs.SetInt("highScore", _currentScore);
                endFinalScore.text = "New HIGH SCORE: " + _currentScore;
            }
            else
            {
                endFinalScore.text = "High Score: " + highScore + "\n" + "Final Score: " + _currentScore;
            }
        }
        
    }
}
