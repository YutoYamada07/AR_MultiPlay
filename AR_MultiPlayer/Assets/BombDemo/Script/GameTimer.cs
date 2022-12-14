using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameTimer : MonoBehaviour
{
    [Header("Reference")]
    public Text countdownText;
    public GameObject timerBlock;

    [Header("Timer")]
    public bool autoStart;
    public bool counting;
    public float countingDownNow;
    public float countDownTotal = 60;
    public float normalizedTime;

    // Use this for initialization
    void OnEnable()
    {
        //Debug.Log("StartCounting");
        if (autoStart)
            StartCounting();
    }
    public void Init()
    {
        countdownText = GameVal.gc.ir.timerText;
        timerBlock = GameVal.gc.ir.timerBlock;
        countdownText.text = countDownTotal.ToString();
    }

    private void OnDisable()
    {
        StopCounting();
        countingDownNow = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (counting)
            Counting();
    }

    public void StopCounting()
    {
        if (countdownText != null)
            countdownText.text = "0";
        if (timerBlock != null)
            timerBlock.SetActive(false);
        counting = false;
        if (timerBlock != null)
            timerBlock.SetActive(counting);
    }

    public void StartCounting()
    {
        if (timerBlock != null)
            timerBlock.SetActive(true);
        counting = true;
        if (timerBlock != null)
            timerBlock.SetActive(counting);
    }

    public void Counting()
    {
        //count reached
        if (countingDownNow < countDownTotal)
        {
            countingDownNow += Time.deltaTime;
            normalizedTime = countingDownNow / countDownTotal;

            //Debug.Log("Time : " + (countDownTotal - countingDownNow));
            if (countdownText != null)
                countdownText.text = Mathf.Round(countDownTotal - countingDownNow).ToString();
        }
        else
        {
            //Game End
            GameVal.gc.GameResult();

            // Remove the recorded 2 seconds.
            countingDownNow = countingDownNow - countDownTotal;

            StopCounting();
        }

    }
    public string FormatTime(float time)
    {
        int minutes = Mathf.RoundToInt(time) / 60;
        int seconds = Mathf.RoundToInt(time) - 60 * minutes;
        int milliseconds = Mathf.RoundToInt((1000 * (time - minutes * 60 - seconds)));
        //return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
