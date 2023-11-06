using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCardRender : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyCards;
    [SerializeField] Text enemyCardText;
    [SerializeField] public bool isShow = false;
    [SerializeField] public GameObject cardBackprefab;

    public int enemyCard;
    FieldManager fieldManager;

    // Update is called once per frame
    void Update()
    {
        //enemyCard = fieldManager.enemyCardNum;

        ShowNumber();
    }

    public void BoolChange()
    {
        if (isShow)
        {
            isShow = false;
            return;
        }
        else
        {
            isShow = true;
            return;
        }
    }

    private void ShowNumber()
    {
        if (isShow)
        {
            enemyCardText.text = enemyCard.ToString();
        }
        else
        {
            enemyCardText.text = "";
        }
    }

    private void BackwordCardSet()
    {
        //기본 카드 세팅
        for (int i = 0; i < enemyCard; i++)
        {
            //enemyCards[i].SetActive(true);
        }
    }

    void AddBackwordCard()
    {
        enemyCards.Add(cardBackprefab);
    }

    void RemoveBackwordCard()
    {
        int i = enemyCards.Count;
        enemyCards.Remove(enemyCards[i]);
    }
}
