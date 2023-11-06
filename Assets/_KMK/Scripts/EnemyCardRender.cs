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
    [SerializeField] Text enemyCardText;
    [SerializeField] public bool isShow = false;
    [SerializeField] public GameObject cardBackPrefab;
    [SerializeField] private Transform cardTransform;
    [SerializeField] public int count;
    [SerializeField] public float xPos;
    [SerializeField] public float yPos;

    FieldManager fieldManager;

    private void Start()
    {
        fieldManager = FindObjectOfType<FieldManager>();
    }

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
            enemyCardText.text = count.ToString();
        }
        else
        {
            enemyCardText.text = "";
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
