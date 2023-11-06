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

    bool canPlace = true;
    private Vector2 instantiatePosition;

    const int FULL_FIELD_COUNT = 10;

    Field tmpField;
    [SerializeField] GameObject directionCanvas;

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
            if (canPlace)
            {
                if (rayhit.collider != null)
                {
                    if (rayhit.collider.GetComponent<Field>() != null)
                    {
                        tmpField = rayhit.collider.GetComponent<Field>();
                        SelectDirection(tmpField);
                    }
                }
            }
            else
            {
                UnitObject unitData = rayhit.collider.GetComponent<Field>().getFieldUnit();
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

    void SelectDirection(Field field)
    {
        directionCanvas.transform.position = field.transform.position;
        directionCanvas.SetActive(true);
        canPlace = false;
    }

    void PlaceCard(Field field, bool lookingLeft)
    {
        if (HandManager.Instance.selectedHand == null) return;
        if (field.isEmpty)
        {
            AddUnit(field.gameObject, HandManager.Instance.selectedHand.card);
            field.SetCard(HandManager.Instance.selectedHand.card, lookingLeft ); 
        }
        else
        {
            if (IsFieldFull()) return;
            fields.Add(field.gameObject);
            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
            newField.GetComponent<Field>().SetCard(HandManager.Instance.selectedHand.card, lookingLeft);
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
                    Debug.LogError(e.Message);
                    break;
                }
            }
        }
        directionCanvas.SetActive(false);
        canPlace = true;
        HandManager.Instance.RemoveHand();
    }

    bool IsFieldFull()
    {
        return fields.Count == FULL_FIELD_COUNT;
    }

    public void ResetAllField()
    {
        int i = 0;
        Field tmpField = battleFields.First;

        for (; i<5; i++)
        {
            tmpField = tmpField.Next;
        }
        while(tmpField != null)
        {
            battleFields.Remove(tmpField);
            fields.Remove(tmpField.gameObject);
            Destroy(tmpField.gameObject);
            tmpField = tmpField.Next;
        }
    }

    /// <summary>
    /// OnClick용도의 함수
    /// </summary>
    /// <param name="lookingLeft"></param>
    public void SelectDirection(bool lookingLeft)
    {
        PlaceCard(tmpField, lookingLeft);
    }
}
