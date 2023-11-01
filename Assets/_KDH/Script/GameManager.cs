using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Card> fieldCards;
    public List<Field> fields;
    public bool red;
    public Dictionary<Field, Card> cardPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fieldCards = new List<Card>();
        red = false;
        cardPosition = new Dictionary<Field, Card>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TurnAct());
        }
    }

    public void AddCard(Card card, Field field)
    {
        fieldCards.Add(card);
        cardPosition.Add(field, card);
        red = !red;
    }

    //public void TurnAction()
    //{
    //    if (cardPosition[fields[0]].lookingLeft)
    //    {
    //        if (cardPosition.ContainsKey(fields[0].prevField))
    //        {
    //            Battle(cardPosition[fields[0]], cardPosition[fields[0].prevField], fields[0], fields[0].prevField);
    //        }
    //    }
    //    else
    //    {
    //        if (cardPosition.ContainsKey(fields[0].nextField))
    //        {
    //            Battle(cardPosition[fields[0]], cardPosition[fields[0].nextField], fields[0], fields[0].nextField);
    //        }
    //    }
    //}

    IEnumerator TurnAct()
    {
        for(int i = 0; i < fields.Count; i++)
        {
            Debug.Log(i);
            if (cardPosition[fields[i]].lookingLeft)
            {
                if (fields[i].prevField != null && cardPosition.ContainsKey(fields[i].prevField))
                {
                    Battle(cardPosition[fields[i]], cardPosition[fields[i].prevField], fields[i], fields[i].prevField);
                }
            }
            else
            {
                if (fields[i].nextField != null && cardPosition.ContainsKey(fields[i].nextField))
                {
                    Battle(cardPosition[fields[i]], cardPosition[fields[i].nextField], fields[i], fields[i].nextField);
                }
            }

            yield return new WaitForSeconds(5);
        }
    }

    private void Battle(Card card1, Card card2, Field field1, Field field2)
    {
        int card1Attack = card1.frontDamage;
        int card2Attack = (card1.lookingLeft != card2.lookingLeft) ? card2.frontDamage : card2.backDamage;

        if (card1Attack > card2Attack)
        {
            field2.card = null;
            field2.spriteRenderer.sprite = null;
        }
        else if (card1Attack < card2Attack)
        {
            field1.card = null;
            field1.spriteRenderer.sprite = null;
        }
    }
}
