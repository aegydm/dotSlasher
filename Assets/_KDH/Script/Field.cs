using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using TMPro;

[RequireComponent(typeof(UnitObject))]
public class Field : MonoBehaviour
{
    public bool isEmpty = true;
    public int fieldOrder;
    public Card card;
    public SpriteRenderer spriteRenderer;
    public Field Prev;
    public Field Next;

    public bool canBattle
    {
        get
        {
            return _canBattle;
        }
        set
        {
            _canBattle = value;
        }
    }
    public UnitObject unitObject;

    private bool _canBattle = false;

    [SerializeField] TMP_Text frontDamageText;
    [SerializeField] TMP_Text backDamageText;
    [SerializeField] TMP_Text orderText;

    private void Awake()
    {
        unitObject = GetComponent<UnitObject>();
        unitObject.spriteRenderer = spriteRenderer;
    }

    private void Start()
    {
    }

    //public void Attack(LinkedBattleField battleField)
    //{
    //    unitObject.cardData.AttackStart(battleField, this);
    //}

    //public void Damaged(int damageVal = 0)
    //{
    //    if (unitObject.cardData.cardCategory == CardCategory.hero)
    //    {
    //        unitObject.cardData.GetDamage(ref damageVal);
    //    }
    //    else if (unitObject.cardData.cardCategory == CardCategory.minion)
    //    {
    //        unitObject.CardChange(new Card());
    //        ResetField();
    //    }
    //    else
    //    {
    //        Debug.LogError("�������� ������ �� �����ϴ�");
    //    }
    //}

    public void ResetField()
    {
        isEmpty = true;
        card = new Card();
        spriteRenderer.sprite = null;
        canBattle = false;
        frontDamageText.text = string.Empty;
        backDamageText.text = string.Empty;
    }

    public void SetCard(Card newCard, bool lookLeft = false)
    {
        int tmpCount = BattleManager.instance.unitList.Count;
        if (isEmpty)
        {
            card = newCard;
            isEmpty = false;
            unitObject.lookingLeft = lookLeft;
            unitObject.Setting(newCard);
            spriteRenderer.sprite = card.cardSprite;
            fieldOrder = tmpCount;
            if (lookLeft == false)
            {
                frontDamageText.text = card.frontDamage.ToString();
                backDamageText.text = card.backDamage.ToString();
            }
            else
            {
                frontDamageText.text = card.backDamage.ToString();
                backDamageText.text = card.frontDamage.ToString();
            }
            orderText.text = fieldOrder.ToString();
        }
    }

    /// <summary>
    /// ����� �� ����
    /// �ʵ� �ϼ����� �����ϴ� ����� ���鶧 �����͸� ��ȯ���ִ� ���·� �Ϸ��ߴµ� �����԰� ���ǰ� �ʿ�.
    /// �Ƹ��� unitObject�� ��ȯ�� �ʿ��ҵ��ؼ� UnitObject��ȯ������ �ӽ÷� ����ٰ� �ߴ�.
    /// </summary>
    /// <returns></returns>
    public UnitObject getFieldUnit()
    {
        return unitObject;
    }
}
