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
    //IEnumerator timerCoroutine;

    private void Start()
    {
        GameManager.instance.CallTurnStart += PlayerTimer;
        GameManager.instance.CallTurnEnd += StopTimer;
        //timerCoroutine = TurnTimer();
    }

    public void PlayerTimer()
    {
        Debug.Log("Callback Timer");
        if (dirty == false)
        {
            Debug.Log("Start Timer");
            if (GameManager.instance.canAct || GameManager.instance.gamePhase == GamePhase.DrawPhase || GameManager.instance.gamePhase == GamePhase.ExecutionPhase)
            {
                timerObj.SetActive(true);
                baseTimer = 30.0f;
                dirty = true;
                StartCoroutine(nameof(TurnTimer));
            }
        }
    }

    public void StopTimer()
    {
        StopCoroutine(nameof(TurnTimer));
        timerObj.SetActive(false);
        dirty = false;
        baseTimer = 30.0f;
    }

    private IEnumerator TurnTimer()
    {
        while (baseTimer > 0)
        {
            baseTimer -= Time.deltaTime;

            timer.text = baseTimer.ToString("F0");

            yield return null;
        }

        GameManager.instance.EndButton();
        StopTimer();
    }
}
