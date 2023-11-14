using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject timerObj;
    private TMP_Text timer;
    private float baseTimer = 30.0f;
    private float runTime;

    private void Awake()
    {
        timer = timerObj.GetComponent<TMP_Text>();
        GameManager.instance.CallTurnStart += PlayerTimer;
    }


    public void PlayerTimer()
    {
        StartCoroutine(TurnTimer());
    }

    public void StopTimer()
    {
        timerObj.SetActive(false);
        StopCoroutine(TurnTimer());
    }

    private IEnumerator TurnTimer()
    {
        timerObj.SetActive(true);

        baseTimer = 30.0f;

        while (baseTimer > 0)
        {
            runTime = Time.deltaTime;
            baseTimer -= runTime;

            timer.text = baseTimer.ToString("F0");

            yield return null;
        }

        GameManager.instance.EndButton();
        timerObj.SetActive(false);
    }
}
