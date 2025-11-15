using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float timeRemaining = 0f;
    public Text timerText;
    public bool endGame = false;
    
    void Update()
    {
        if (!endGame)
        {
            timeRemaining += Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
            
        }
        
    }

    public void LostGame()
    {
        Debug.Log("Lost Game");
        endGame = true;
    }
    
}

