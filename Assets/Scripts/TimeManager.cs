using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float timeRemaining = 0f;
    public Text timerText;

    void Update()
    {
        timeRemaining += Time.deltaTime;
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }
    
    
}

