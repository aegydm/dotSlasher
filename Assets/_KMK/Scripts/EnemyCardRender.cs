using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCardRender : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyCards;
    [SerializeField] TMP_Text enemyCardText;
    //[SerializeField] public bool isShow = false;
    [SerializeField] public GameObject cardBackPrefab;
    [SerializeField] private Transform cardTransform;
    [SerializeField] public int count;
    [SerializeField] public float xPos;
    [SerializeField] public float yPos;

    //FieldManager fieldManager;

    //private void Start()
    //{
    //    fieldManager = FieldManager.Instance;
    //    fieldManager.OnEnemyHandChanged += ShowNumber;
    //    fieldManager.OnEnemyHandChanged += RenderEnemyCard;
    //}

    private void ShowNumber(int cardNum)
    {
        if (cardNum >= 0)
        {
            enemyCardText.text = cardNum.ToString();
        }
        else
        {
            enemyCardText.text = "";
            Debug.LogError("EnemyCard can't down under 0");
        }
    }

    public void RenderEnemyCard(int cardNum)
    {
        while(cardNum > count)
        {
            BackwordCardAdd();
        }
        while(cardNum < count)
        {
            RemoveCard();
        }
    }

    public void BackwordCardAdd()
    {
        count = enemyCards.Count;

        Vector3 spawnPos = new Vector3(count * xPos, count * yPos, 0);

        Vector3 vector3 = cardTransform.position;

        Vector3 newVector = vector3 + spawnPos;

        GameObject instansceObj = Instantiate(cardBackPrefab, newVector, Quaternion.identity);

        enemyCards.Add(instansceObj);

        count = enemyCards.Count;
    }

    public void RemoveCard()
    {
        Destroy(enemyCards[enemyCards.Count- 1]);
        enemyCards.RemoveAt(enemyCards.Count - 1);
        count = enemyCards.Count;
    }
}
