using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject timerObj;
    public TMP_Text timer;
    private float baseTimer = 30.0f;
    private float runTime;
    private bool dirty = false;
    IEnumerator timerCoroutine;

    private void Start()
    {
        GameManager.instance.CallTurnStart += PlayerTimer;
        GameManager.instance.CallTurnEnd += StopTimer;
        timerCoroutine = TurnTimer();
    }


    public void PlayerTimer()
    {
        if (dirty == false)
        {
            timerObj.SetActive(true);
            baseTimer = 30.0f;
            dirty = true;
            StartCoroutine(timerCoroutine);
        }
    }

    public void StopTimer()
    {
        timerObj.SetActive(false);
        StopCoroutine(timerCoroutine);
        dirty = false;
        baseTimer = 30.0f;
    }

    private IEnumerator TurnTimer()
    {
        timerObj.SetActive(true);
        baseTimer = 30.0f;

        while (baseTimer > 0)
        {
            Debug.Log(Time.deltaTime);
            baseTimer -= Time.deltaTime;

            timer.text = baseTimer.ToString("F0");

            yield return null;
        }

        GameManager.instance.EndButton();
        timerObj.SetActive(false);
        dirty = false;
    }
}
