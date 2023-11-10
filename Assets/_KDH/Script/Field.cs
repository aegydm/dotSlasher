using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(UnitObject))]
public class Field : MonoBehaviour
{
    public bool isNewField = false;
    public bool blankField = false;
    public bool isEmpty = true;
    public int fieldOrder;
    public Card card;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
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
            if(canBattle == false)
            {
                if (animator != null && animator.runtimeAnimatorController != null)
                {
                    Debug.Log("Idle");
                    animator.Play("Idle");
                }
            }
        }
    }
    public UnitObject unitObject;

    private bool _canBattle = false;

    [SerializeField] TMP_Text frontDamageText;
    [SerializeField] TMP_Text backDamageText;
    public TMP_Text orderText;
    public Image playerColor;

    private void Awake()
    {
        unitObject = GetComponent<UnitObject>();
        unitObject.spriteRenderer = spriteRenderer;
        unitObject.animator = animator;
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
    //        Debug.LogError("아이템을 공격할 수 없습니다");
    //    }
    //}

    public void ResetField()
    {
        unitObject.playerID = "-1";
        isEmpty = true;
        animator.runtimeAnimatorController = null;
        spriteRenderer.sprite = null;
        canBattle = false;
        frontDamageText.text = string.Empty;
        backDamageText.text = string.Empty;
        unitObject.cardData = card = new Card();
        orderText.text = string.Empty;
        orderText.color = Color.black;
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
    /// 만들다 만 상태
    /// 필드 하수인을 지정하는 기능을 만들때 데이터를 반환해주는 형태로 하려했는데 대현님과 상의가 필요.
    /// 아마도 unitObject의 반환이 필요할듯해서 UnitObject반환값으로 임시로 만들다가 중단.
    /// </summary>
    /// <returns></returns>
    public UnitObject getFieldUnit()
    {
        return unitObject;
    }
}
