using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CCGCard;
using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FieldManager : MonoBehaviour
{
    public List<GameObject> fields;
    public static FieldManager Instance;
    public GameObject FieldPrefab;
    public LinkedBattleField battleFields;
    public List<GameObject> newFields;
    public bool isLeftForPun;
    public bool cancelTrigger = false;
    public bool canPlace
    {
        get
        {
            return _canPlace;
        }
        set
        {
            _canPlace = value;
        }
    }

    private bool _canPlace;

    public int enemyCardNum
    {
        get
        {
            return _enemyCardNum;
        }
        set
        {
            _enemyCardNum = value;
            OnEnemyHandChanged?.Invoke(enemyCardNum);
        }
    }

    private int _enemyCardNum;
    public Vector2 instantiatePosition;
    public Vector2 mousePos;
    public Vector2 tilePos;

    public event Action<int> OnEnemyHandChanged;

    const int FULL_FIELD_COUNT = 10;

    Field tmpField;
    [SerializeField] GameObject directionCanvas;
    [SerializeField] TMP_Text enemyHandCount;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        OnEnemyHandChanged += EnemyHandCountRender;
    }

    void EnemyHandCountRender(int count)
    {
        enemyHandCount.text = $"EnemyHand : {count}";
    }

    private void Start()
    {
        for (int i = 0; i < fields.Count; i++)
        {
            battleFields.Add(fields[i]);
        }
        canPlace = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && GameManager.Instance.gamePhase == GamePhase.ActionPhase && GameManager.Instance.canAct && cancelTrigger)
        {
            Cancel();
        }
    }

    public void AddUnit(GameObject GO, Card cardData, int id = -1)
    {
        //battleFields.Find(GO).unitObject.CardChange(cardData);
        //if (cardData.cardName != string.Empty)
        //{
        //    battleFields.Find(GO).canBattle = true;
        //}
        //BattleManager.instance.unitList.Add(battleFields.Find(GO));
        //if (id == -1)
        //{
        //    battleFields.Find(GO).unitObject.playerID = GameManager.Instance.playerID;
        //}
        //else
        //{
        //    battleFields.Find(GO).unitObject.playerID = id.ToString();
        //}
    }
    void SelectDirection(Field field)
    {
        if (HandManager.Instance.selectedHand == null) return;
        directionCanvas.transform.position = field.transform.position;
        directionCanvas.SetActive(true);
        tilePos = field.transform.position;
        canPlace = false;
    }

    public bool SelectField(Field field, bool isLeft)
    {
        //if (field.isEmpty)
        //{
        //    tmpField = field;
        //    cancelTrigger = true;
        //    SelectDirection(field);
        //    mousePos = field.transform.position;
        //    tilePos = field.transform.position;
        //    return true;
        //}
        //else
        //{
        //    if (isLeft)
        //    {
        //        if (field.Prev != null && field.Prev.isEmpty)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            if (IsFieldFull())
        //            {
        //                return false;
        //            }
        //            isLeftForPun = isLeft;
        //            Debug.LogError(field.name + "좌측" + (isLeft ? "좌측" : "우측"));

        //            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
        //            newField.GetComponent<Field>().isNewField = true;
        //            newFields.Add(newField);
        //            battleFields.AddBefore(field, newField);
        //            this.tmpField = battleFields.Find(newField);

        //            // Draw field in screen
        //            Field tmpField = battleFields.First;
        //            fields.Clear();
        //            while (tmpField != null)
        //            {
        //                fields.Add(tmpField.gameObject);
        //                tmpField = tmpField.Next;
        //            }
        //            for (int pos = (fields.Count - 1) * -9, i = 0; i < fields.Count; pos += 18, i++)
        //            {
        //                try
        //                {
        //                    fields[i].transform.position = new Vector3(pos, 0, 0);
        //                }
        //                catch (Exception e)
        //                {
        //                    Debug.LogError(e.Message);
        //                    break;
        //                }
        //            }
        //            cancelTrigger = true;
        //            tilePos = newField.transform.position;
        //            SelectDirection(this.tmpField);
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        if (field.Next != null && field.Next.isEmpty)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            if (IsFieldFull())
        //            {
        //                return false;
        //            }
        //            Debug.LogError("우측" + (isLeft ? "좌측" : "우측"));

        //            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
        //            newField.GetComponent<Field>().isNewField = true;
        //            newFields.Add(newField);
        //            battleFields.AddAfter(field, newField);
        //            this.tmpField = battleFields.Find(newField);

        //            // Draw field in screen
        //            Field tmpField = battleFields.First;
        //            fields.Clear();
        //            while (tmpField != null)
        //            {
        //                fields.Add(tmpField.gameObject);
        //                tmpField = tmpField.Next;
        //            }
        //            for (int pos = (fields.Count - 1) * -9, i = 0; i < fields.Count; pos += 18, i++)
        //            {
        //                try
        //                {
        //                    fields[i].transform.position = new Vector3(pos, 0, 0);
        //                }
        //                catch (Exception e)
        //                {
        //                    Debug.LogError(e.Message);
        //                    break;
        //                }
        //            }
        //            cancelTrigger = true;
        //            tilePos = newField.transform.position;
        //            SelectDirection(this.tmpField);
        //            return true;
        //        }
        //    }
        //}
        return true;
    }

    public void PlaceCard(Field field, Card card, int id, bool lookLeft)
    {
        if (card == null) return;
        if (field.isEmpty)
        {
            AddUnit(field.gameObject, card, id);
            field.SetCard(card, lookLeft);
        }
        directionCanvas.SetActive(false);
        canPlace = true;
    }

    public bool IsFieldFull()
    {
        return fields.Count >= FULL_FIELD_COUNT;
    }

    public void ResetAllField()
    {
        //Field tmpField = battleFields.First;
        ////for (int i = 0; i < 5; i++)
        ////{
        ////    tmpField = tmpField.Next;
        ////}
        //for (int i = 0; i < newFields.Count; i++)
        //{
        //    if (newFields.Count > 0 && newFields[i] != null)
        //    {
        //        battleFields.Remove(newFields[i]);
        //        fields.Remove(newFields[i]);
        //        Destroy(newFields[i]);
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}

        //newFields.Clear();


        //for (int pos = (fields.Count - 1) * -9, i = 0; i < fields.Count; pos += 18, i++)
        //{
        //    try
        //    {
        //        fields[i].transform.position = new Vector3(pos, 0, 0);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e.Message);
        //        break;
        //    }
        //}
    }

    /// <summary>
    /// OnClick용도의 함수
    /// </summary>
    /// <param name="lookingLeft"></param>
    public void SelectDirection(bool lookingLeft)
    {
        GameManager.Instance.canAct = false;

        GameManager.Instance.photonView.RPC("MakeFieldAndSetCardForPun", RpcTarget.Others, mousePos, instantiatePosition, isLeftForPun, HandManager.Instance.selectedHand.card.cardID, int.Parse(GameManager.Instance.playerID), lookingLeft);
        GameManager.Instance.PlaceCardForPun(tilePos, HandManager.Instance.selectedHand.card.cardID, int.Parse(GameManager.Instance.playerID), lookingLeft);
        HandManager.Instance.RemoveHand();
    }

    /// <summary>
    /// 필드 선택 취소
    /// </summary>
    public void Cancel()
    {
        //if (cancelTrigger)
        //{
        //    Debug.Log("Canceling");
        //    if (tmpField.isNewField)
        //    {
        //        battleFields.Remove(tmpField);
        //        fields.Remove(tmpField.gameObject);
        //        Destroy(tmpField.gameObject);
        //    }

        //    for (int pos = (fields.Count - 1) * -9, i = 0; i < fields.Count; pos += 18, i++)
        //    {
        //        try
        //        {
        //            fields[i].transform.position = new Vector3(pos, 0, 0);
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.LogError(e.Message);
        //            break;
        //        }
        //    }
        //    canPlace = true;
        //    directionCanvas.SetActive(false);
        //    cancelTrigger = false;
        //    HandManager.Instance.selectedHand.isSelected = !HandManager.Instance.selectedHand.isSelected;
        //    HandManager.Instance.selectedHand = null;
        //}
    }

    public void TurnEnd()
    {
        if (GameManager.Instance.canAct)
        {
            GameManager.Instance.canAct = false;
        }
        GameManager.Instance.playerEnd = true;
    }
}
