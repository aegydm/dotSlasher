using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class FieldCardObjectTest : MonoBehaviour
{
    public Card cardData
    {
        get
        {
            return _cardData;
        }
        set
        {
            if ((cardData != null) && (cardData != new Card()) && (value == null || value == new Card()))
            {
                _cardData = value;
                isEmpty = true;
                GetComponent<BoxCollider2D>().enabled = true;
                //rightInter.SetActive(false);
                ResetRender();
            }
            else if (cardData != value && value != null && value != new Card())
            {
                _cardData = value;
                isEmpty = false;
                GetComponent<BoxCollider2D>().enabled = false;
                RenderCard();
            }
        }
    }

    [Header("필드에 들어있는 카드의 정보")]
    [SerializeField] private Card _cardData;
    [Header("필드가 비어있는지 체크하는 변수")]
    public bool isEmpty = true;
    [Header("필드 전체 틀 스프라이트")]
    [SerializeField] SpriteRenderer fieldSprite;
    [Header("실제 카드의 스프라이트")]
    public SpriteRenderer cardSprite;
    public Animator animator;
    [Header("카드의 공격력 표시용")]
    public TMP_Text frontATKText;
    public TMP_Text backATKText;

    public void CheckInter()
    {
        if (isEmpty == false && this == FieldManagerTest.instance.battleField.First)
        {
            leftInter.SetActive(true);
        }
        else
        {
            leftInter.SetActive(false);
        }
        if (isEmpty == false && (Next == null || Next.isEmpty == false))
        {
            rightInter.SetActive(true);
        }
        else
        {
            rightInter.SetActive(false);
        }
    }

    public bool lookingLeft
    {
        get
        {
            return _lookingLeft;
        }
        set
        {
            if (_lookingLeft != value)
            {
                cardSprite.flipX = value;
                _lookingLeft = value;
                frontATKText.text = _lookingLeft ? cardData.backDamage.ToString() : cardData.frontDamage.ToString();
                backATKText.text = _lookingLeft ? cardData.frontDamage.ToString() : cardData.backDamage.ToString();
            }
        }
    }
    [Header("카드의 좌우")]
    [SerializeField] private bool _lookingLeft = false;
    [Header("필드 소유자의 ID")]
    public int playerID;
    [Header("카드의 소유자 표시용 이미지")]
    public Image ownerColor;

    public bool canBattle
    {
        get
        {
            return _canBattle;
        }
        set
        {
            if (animator.runtimeAnimatorController != null && value == true)
            {
                animator.Play("Idle");
            }
            _canBattle = value;
        }
    }
    [Header("해당 필드의 공격 가능 여부")]
    [SerializeField] private bool _canBattle;
    [Header("공격 가능 여부 표시용 이미지")]
    public Image canBattleImage;
    [Header("좌측 끼어들기 용 - First일 때만 활성화 예정")]
    [SerializeField] GameObject leftInter;
    [Header("우측 끼어들기 가능 영역 표시용")]
    [SerializeField] GameObject rightInter;
    [Header("이전 칸과 다음 칸의 정보")]
    public FieldCardObjectTest Prev;
    public FieldCardObjectTest Next;

    private void RenderCard()
    {
        cardSprite.sprite = cardData.cardSprite;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(cardData.animator);
        frontATKText.text = _lookingLeft ? cardData.backDamage.ToString() : cardData.frontDamage.ToString();
        backATKText.text = _lookingLeft ? cardData.frontDamage.ToString() : cardData.backDamage.ToString();
    }

    private void ResetRender()
    {
        animator.runtimeAnimatorController = null;
        cardSprite.sprite = null;
        frontATKText.text = string.Empty;
        backATKText.text = string.Empty;
    }

    private void OnMouseOver()
    {
        if (isEmpty && PlayerActionManager.instance.isDrag && TestManager.instance.canAct && UIManager.Instance.isPopUI == false)
        {
            PlayerActionManager.instance.field = this;
        }
    }

    private void OnMouseExit()
    {
        if (PlayerActionManager.instance.isDrag)
            CancelFieldSelect();
    }

    public void CancelFieldSelect()
    {
        if (PlayerActionManager.instance.isDrag || PlayerActionManager.instance.dirtyForInter == false)
        {
            PlayerActionManager.instance.field = null;
        }
    }

    private void OnMouseDown()
    {
        if (cardData != null && cardData.cardID != 0 && canBattle && TestManager.instance.isBattle && TestManager.instance.canAct && UIManager.Instance.isPopUI == false)
        {
            Debug.Log("전투 시작");
            TestManager.instance.isBattle = false;
            cardData.AttackStart(FieldManagerTest.instance.battleField, this);
            Debug.Log("마우스가 눌렸다.");
        }
    }
}
