using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float timeRemaining = 0f;
    public Text timerText;
    public bool endGame = false;
    public Canvas endgameCanvas;
    public Text endgameText;

    private void Awake()
    {
        endgameCanvas.enabled = false;
    }

    void Update()
    {
        if (!endGame)
        {
            timeRemaining += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void LostGame()
    {
        endgameCanvas.enabled =  true;
        endgameText.text = "GAME OVER";
        endGame = true;
    }
    
}

