using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUi : MonoBehaviour
{
    public float gameTime;
    float currentTime;
    Image timerImage;

    private void Awake()
    {
        timerImage = GetComponent<Image>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        timerImage.fillAmount = currentTime / gameTime;

        if (currentTime >= gameTime)
        {
            // game over
        }
    }
}
