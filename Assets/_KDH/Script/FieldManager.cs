using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CCGCard;
using System;

public class FieldManager : MonoBehaviour
{
    public List<GameObject> fields;
    public static FieldManager Instance;
    public GameObject FieldPrefab;
    public LinkedBattleField battleFields;
    private Vector2 instantiatePosition;

    const int FULL_FIELD_COUNT = 10;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            instantiatePosition = new Vector3(mousePos.x, 0, 0);
            RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (rayhit.collider != null)
            {
                if (rayhit.collider.GetComponent<Field>() != null)
                {
                    PlaceCard(rayhit.collider.GetComponent<Field>());
                }
            }
        }
    }
    public void AddUnit(GameObject GO, Card cardData)
    {
        battleFields.Find(GO).unitObject.CardChange(cardData);
        if (cardData.cardName != string.Empty)
        {
            battleFields.Find(GO).canBattle = true;
        }
        BattleManager.instance.unitList.Add(battleFields.Find(GO));
    }

    void PlaceCard(Field field)
    {
        if (HandManager.Instance.selectedHand == null) return;
        if (field.isEmpty)
        {
            AddUnit(field.gameObject, HandManager.Instance.selectedHand.card);
            field.SetCard(HandManager.Instance.selectedHand.card); 
        }
        else
        {
            if (IsFieldFull()) return;
            fields.Add(field.gameObject);
            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
            newField.GetComponent<Field>().SetCard(HandManager.Instance.selectedHand.card);
            //newField.GetComponent<Field>().Prev = field.Prev;
            //newField.GetComponent<Field>().Next = field;
            //field.Prev.Next = newField.GetComponent<Field>();
            //field.Prev = newField.GetComponent<Field>();  
            battleFields.AddBefore(field, newField);
            Field tmpField = battleFields.First;
            fields.Clear();
            while (tmpField != null)
            {
                fields.Add(tmpField.gameObject);
                tmpField = tmpField.Next;
            }
            for(int pos = (fields.Count-1) * -9, i = 0; ; pos+=18, i++)
            {
                try
                {
                    fields[i].transform.position = new Vector3(pos, 0, 0);
                }
                catch(Exception e)
                {
                    break;
                }
            }
        }
        HandManager.Instance.RemoveHand();
    }

    bool IsFieldFull()
    {
        return fields.Count == FULL_FIELD_COUNT;
    }
}
