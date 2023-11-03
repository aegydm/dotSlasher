using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Result
{
    Win,
    Lose,
    Draw
}
public enum Hand
{
    Rock,
    Scissor,
    Paper
}

public class RSP : MonoBehaviour
{
    public Result result;
    public Hand[] hands;
    public int playerNum;
    public bool solo;
    public bool trigger;
    public bool trigger2;
    public bool startFirst;
    [SerializeField] Text resultText;
    [SerializeField] Text cheatText;
    [SerializeField] GameObject choose;
    [SerializeField] Button[] buttons;
    [SerializeField] Text chooseText;
    [SerializeField] Text cheatText2;

    private void Start()
    {
        startFirst = true;
        solo = true;
        trigger = true;
        hands = new Hand[playerNum];
        trigger2 = Random.Range(0, 100) <= 49 ? true : false;
        if (trigger2)
        {
            cheatText2.text = "����";
        }
        else
        {
            cheatText2.text = "�İ�";
        }

        if (solo)
        {
            int randNum = Random.Range(0, 99);
            if (randNum < 33)
            {
                for (int i = 1; i < playerNum; i++)
                {
                    hands[i] = Hand.Rock;
                }
                cheatText.text = "����";
            }
            else if (randNum < 66)
            {
                for (int i = 1; i < playerNum; i++)
                {
                    hands[i] = Hand.Scissor;
                }
                cheatText.text = "����";
            }
            else
            {
                for (int i = 1; i < playerNum; i++)
                {
                    hands[i] = Hand.Paper;
                }
                cheatText.text = "��";
            }
        }
    }

    private void Update()
    {
        if (solo)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                for(int i = 1; i < playerNum; i++)
                {
                    hands[i] = Hand.Rock;
                }
                cheatText.text = "����";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                for (int i = 1; i < playerNum; i++)
                {
                    hands[i] = Hand.Scissor;
                }
                cheatText.text = "����";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                for (int i = 1; i < playerNum; i++)
                {
                    hands[i] = Hand.Paper;
                }
                cheatText.text = "��";
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                trigger2 = true;
                cheatText2.text = "����";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                trigger2 = false;
                cheatText2.text = "�İ�";
            }
        }
    }

    public Result RockScissorPaper()
    {
        int rCount = 0;
        int sCount = 0;
        int pCount = 0;
        foreach(Hand hand in hands)
        {
            if(hand == Hand.Rock)
            {
                rCount++;
            }
            else if(hand == Hand.Scissor)
            {
                sCount++;
            }
            else if( hand == Hand.Paper)
            {
                pCount++;
            }
        }

        if (rCount != 0 && sCount != 0 && pCount != 0)
        {
            return Result.Draw;
        }
        else if ((rCount != 0 && sCount == 0 && pCount == 0) || (sCount != 0 && rCount == 0 && pCount == 0) || (pCount != 0 && rCount == 0 && sCount == 0))
        {
            return Result.Draw;
        }
        else if ((hands[0] == Hand.Rock && sCount != 0) || (hands[0] == Hand.Scissor && pCount != 0) || (hands[0] == Hand.Paper && rCount != 0))
        {
            return Result.Win;
        }
        else
        {
            return Result.Lose;
        }
    }

    public void Rock()
    {
        if (trigger)
        {
            hands[0] = Hand.Rock;
            Result result = RockScissorPaper();
            if (result == Result.Draw)
            {
                resultText.text = "���º�";
            }
            else if (result == Result.Win)
            {
                resultText.text = "�¸�";
                trigger = false;
                Win();
            }
            else
            {
                resultText.text = "�й�";
                trigger = false;
                Lose();
            }
        }
    }

    public void Scissor()
    {
        if (trigger)
        {
            hands[0] = Hand.Scissor;
            Result result = RockScissorPaper();
            if (result == Result.Draw)
            {
                resultText.text = "���º�";
            }
            else if (result == Result.Win)
            {
                resultText.text = "�¸�";
                trigger = false;
                Win();
            }
            else
            {
                resultText.text = "�й�";
                trigger = false;
                Lose();
            }
        }
    }

    public void Paper()
    {
        if (trigger)
        {
            hands[0] = Hand.Paper;
            Result result = RockScissorPaper();
            if (result == Result.Draw)
            {
                resultText.text = "���º�";
            }
            else if (result == Result.Win)
            {
                resultText.text = "�¸�";
                trigger = false;
                Win();
            }
            else
            {
                resultText.text = "�й�";
                trigger = false;
                Lose();
            }
        }
    }

    public void Win()
    {
        choose.SetActive(true);
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void Lose()
    {
        choose.SetActive(true);
        chooseText.gameObject.SetActive(true);
        chooseText.text = "������ ���� �� �Դϴ�.";
        StartCoroutine(WaitPC());
    }

    IEnumerator WaitPC()
    {
        yield return new WaitForSeconds(5);
        if (trigger2)
        {
            startFirst = true;
            chooseText.text = "�������� �����մϴ�.";
        }
        else
        {
            startFirst = false;
            chooseText.text = "�İ����� �����մϴ�.";
        }
    }

    public void StartFirst()
    {
        startFirst = true;
        foreach (var button in buttons) 
        {
            button.gameObject.SetActive(false);
        }
        chooseText.gameObject.SetActive(true);
        chooseText.text = "�������� �����մϴ�";
    }

    public void StartLast()
    {
        startFirst = false;
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        chooseText.gameObject.SetActive(true);
        chooseText.text = "�İ����� �����մϴ�";
    }
}
