using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CCGCard;

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

    public void AddUnit(GameObject GO, Unit cardData)
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
            AddUnit(field.gameObject, new Unit(HandManager.Instance.selectedHand.card));
            field.SetCard(HandManager.Instance.selectedHand.card);
        }
        else
        {
            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
            newField.GetComponent<Field>().SetCard(HandManager.Instance.selectedHand.card);
            newField.GetComponent<Field>().Prev = field.Prev;
            newField.GetComponent<Field>().Next = field;
            field.Prev.Next = newField.GetComponent<Field>();
            field.Prev = newField.GetComponent<Field>();
            while (field.Next != null)
            {
                field.transform.position = new Vector3(field.transform.position.x + 20, field.transform.position.y);
                field = field.Next;
            }
        }
        HandManager.Instance.RemoveHand();
    }

    bool IsFieldFull()
    {
        if(fields.Count == FULL_FIELD_COUNT)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
