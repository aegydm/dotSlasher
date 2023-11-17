using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Result
{
    Win,
    Lose,
    Draw
}
public enum Hand
{
    Null,
    Rock,
    Scissor,
    Paper
}

public class RSP : MonoBehaviour
{
    public static RSP instance;
    public Result result;
    public readonly int PLAYERNUM = 2;
    public bool trigger;
    public bool startFirst;
    [SerializeField] MatchMaking match;

    public Hand myHand
    {
        get
        {
            return _myHand;
        }
        set
        {
            if (_myHand != value)
            {
                _myHand = value;
                if (_myHand != Hand.Null)
                {
                    CheckHand();
                }
                else
                {
                    trigger = true;
                }
                match.photonView.RPC("SetEnemyHand", RpcTarget.Others, myHand);
            }
        }
    }

    private void CheckHand()
    {
        if (myHand != Hand.Null && enemyHand != Hand.Null)
        {
            Result result = RockScissorPaper(myHand, enemyHand);
            if (result == Result.Draw)
            {
                resultText.text = "Draw";
                Invoke("Draw", 3f);
            }
            else if (result == Result.Win)
            {
                rspObject.SetActive(false);
                Win();
            }
            else if (result == Result.Lose)
            {
                rspObject.SetActive(false);
                Lose();
            }
        }
    }

    private void Draw()
    {
        resultText.text = "Select";
        myHand = Hand.Null;
        enemyHand = Hand.Null;
    }

    public Hand enemyHand
    {
        get
        {
            return _enemyHand;
        }
        set
        {
            if (_enemyHand != value)
            {
                _enemyHand = value;
                if (_enemyHand != Hand.Null)
                {
                    CheckHand();
                }
            }
        }
    }

    [SerializeField] private Hand _myHand = new Hand();
    [SerializeField] private Hand _enemyHand = new Hand();

    [SerializeField] GameObject rspObject;
    [SerializeField] Button[] buttons;
    [SerializeField] TMP_Text resultText;
    [SerializeField] GameObject choose;
    public TMP_Text chooseText;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        startFirst = false;
        trigger = true;
        myHand = Hand.Null;
        enemyHand = Hand.Null;
    }

    public Result RockScissorPaper(Hand _myHand, Hand _enemHand)
    {
        Result result = new Result();
        switch (_myHand)
        {
            case Hand.Rock:
                if (_enemHand == Hand.Rock)
                {
                    result = Result.Draw;
                }
                else if (_enemHand == Hand.Scissor)
                {
                    result = Result.Win;
                }
                else if (_enemHand == Hand.Paper)
                {
                    result = Result.Lose;
                }
                break;
            case Hand.Scissor:
                if (_enemHand == Hand.Rock)
                {
                    result = Result.Lose;
                }
                else if (_enemHand == Hand.Scissor)
                {
                    result = Result.Draw;
                }
                else if (_enemHand == Hand.Paper)
                {
                    result = Result.Win;
                }
                break;
            case Hand.Paper:
                if (_enemHand == Hand.Rock)
                {
                    result = Result.Win;
                }
                else if (_enemHand == Hand.Scissor)
                {
                    result = Result.Lose;
                }
                else if (_enemHand == Hand.Paper)
                {
                    result = Result.Draw;
                }
                break;
            default:
                break;
        }
        return result;
    }

    public void Rock()
    {
        if (trigger)
        {
            resultText.text = "You Choose Rock";
            myHand = Hand.Rock;
            trigger = false;
        }
    }

    public void Scissor()
    {
        if (trigger)
        {
            resultText.text = "You Choose Scissor";
            myHand = Hand.Scissor;
            trigger = false;
        }
    }

    public void Paper()
    {
        if (trigger)
        {
            resultText.text = "You Choose Paper";
            myHand = Hand.Paper;
            trigger = false;
        }
    }

    public void Win()
    {
        choose.SetActive(true);
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(true);
        }
        chooseText.text = "You Win! Select";
    }

    public void Lose()
    {
        choose.SetActive(true);
        chooseText.gameObject.SetActive(true);
        chooseText.text = "You Lose. Wait...";
    }

    public void StartFirst()
    {
        startFirst = true;
        match.photonView.RPC("SetFirst", RpcTarget.Others, false);
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        chooseText.gameObject.SetActive(true);
        chooseText.text = "Start First";
        NetworkManager.instance.StartGame(RSP.instance.startFirst);
    }

    public void StartLast()
    {
        startFirst = false;
        match.photonView.RPC("SetFirst", RpcTarget.Others, true);
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        chooseText.gameObject.SetActive(true);
        chooseText.text = "Start Last";
        NetworkManager.instance.StartGame(RSP.instance.startFirst);
        Instantiate(gameObject, Vector3.zero, Quaternion.identity);
    }
}
