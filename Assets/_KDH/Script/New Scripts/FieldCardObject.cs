using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class FieldCardObject : MonoBehaviour
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
                ResetRender();
                if (GameManager.instance.gamePhase != GamePhase.EndPhase)
                {
                    FieldManager.instance.CallFieldCardRemoved?.Invoke();
                }
            }
            else if (cardData != value && value != null && value != new Card())
            {
                _cardData = value;
                isEmpty = false;
                GetComponent<BoxCollider2D>().enabled = false;
                RenderCard();
            }
            if (cardData != null && cardData != new Card())
            {
                this.playerID = int.Parse(GameManager.instance.playerID);
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

    [Header("Sound")]
    private AudioClip ClickSound;
    public void CheckInter()
    {
        if (FieldManager.instance.FieldIsFull() == false && (FieldManager.instance.GetAdditionalField() != null))
        {

            if (isEmpty == false && this == FieldManager.instance.battleField.First)
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
        else
        {
            leftInter.SetActive(false);
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
                frontATKText.text = _lookingLeft ? (cardData.backDamage).ToString() : (cardData.frontDamage).ToString();
                backATKText.text = _lookingLeft ? (cardData.frontDamage).ToString() : (cardData.backDamage).ToString();
            }
        }
    }
    [Header("카드의 좌우")]
    [SerializeField] private bool _lookingLeft = false;
    [Header("필드 소유자의 ID")]
    [SerializeField] private int _playerID;

    public int playerID
    {
        get
        {
            return _playerID;
        }
        set
        {
            _playerID = value;
            if (playerID != -1)
            {
                ownerColor.color = new Color(255 - (255 * playerID), 255 * playerID, 0);
            }
            else
            {
                ownerColor.color = Color.white;
            }
        }
    }
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
            if (_canBattle != value)
            {
                if (value && attackChance)
                {
                    _canBattle = value;
                    if (animator.runtimeAnimatorController != null)
                    {
                        animator.Play("Idle");
                    }
                }
                else if (value == false)
                {
                    _canBattle = value;
                    if (animator.runtimeAnimatorController != null)
                    {
                        animator.Play("Breath");
                    }
                }
            }
            if (GameManager.instance.gamePhase == GamePhase.BattlePhase || GameManager.instance.nextPhase == GamePhase.ExecutionPhase)
            {
                canBattleImage.color = canBattle ? Color.cyan : Color.white;
            }
        }
    }

    public bool attackChance
    {
        get
        {
            return _attackChance;
        }
        set
        {
            if (animator.runtimeAnimatorController != null && value == true)
            {
                animator.Play("Idle");
            }
            _attackChance = value;
        }
    }

    [Header("해당 필드가 공격을 할 수 있는지 여부")]
    [SerializeField] bool _canBattle = false;
    [Header("해당 필드가 공격권을 가지고 있는지 여부")]
    [SerializeField] private bool _attackChance;
    [Header("공격 가능 여부 표시용 이미지")]
    public Image canBattleImage;
    [Header("좌측 끼어들기 용 - First일 때만 활성화 예정")]
    public GameObject leftInter;
    [Header("우측 끼어들기 가능 영역 표시용")]
    public GameObject rightInter;
    [Header("이전 칸과 다음 칸의 정보")]
    public FieldCardObject Prev;
    public FieldCardObject Next;

    public void RenderCard()
    {
        cardSprite.sprite = cardData.cardSprite;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(cardData.animator);
        frontATKText.text = _lookingLeft ? (cardData.backDamage).ToString() : (cardData.frontDamage).ToString();
        backATKText.text = _lookingLeft ? (cardData.frontDamage).ToString() : (cardData.backDamage).ToString();
        if(cardData.frontDamage != CardDB.instance.FindCardFromID(cardData.cardID).frontDamage)
        {
            frontATKText.color = Color.yellow;
        }
        else
        {
            frontATKText.color = Color.white;
        }
        if(cardData.backDamage != CardDB.instance.FindCardFromID(cardData.cardID).backDamage)
        {
            backATKText.color = Color.yellow;
        }
        else
        {
            backATKText.color = Color.white;
        }
    }

    private void ResetRender()
    {
        animator.runtimeAnimatorController = null;
        cardSprite.sprite = null;
        frontATKText.text = string.Empty;
        backATKText.text = string.Empty;
        frontATKText.color = Color.white;
        backATKText.color = Color.white;
        playerID = -1;
    }

    private void OnMouseOver()
    {
        if (isEmpty && PlayerActionManager.instance.isDrag && GameManager.instance.canAct && UIManager.Instance.isPopUI == false)
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
        //Please Input Card Click Sound Code
        //카드 클릭 사운드 코드 넣어주세요
        //
        //SoundManager.instance.PlayEffSound(ClickSound);
        if (cardData != null && cardData.cardID != 0 && attackChance && GameManager.instance.gamePhase == GamePhase.BattlePhase && GameManager.instance.canAct && UIManager.Instance.isPopUI == false)
        {
            FieldAttack();
        }
    }

    public void FieldAttack()
    {
        if (GameManager.instance.isAlreadyAttack == false && canBattle && playerID == int.Parse(GameManager.instance.playerID))
        {
            GameManager.instance.isAlreadyAttack = true;
            GameManager.instance.photonView.RPC("AttackUnit", Photon.Pun.RpcTarget.All, FieldManager.instance.battleField.FindIndex(this));
            Invoke("DelayTurnEnd", 7);
        }
    }

    private void DelayTurnEnd()
    {
        Debug.LogError("DelayCanAttackFalse");
        GameManager.instance.canAct = false;
        Debug.LogError("DelayIsAttackFalse");
        GameManager.instance.isAlreadyAttack = false;
        Debug.LogError("DelayEnd");
    }

    public void ResetField()
    {
        leftInter.GetComponent<InterField>().ResetInterField();
        rightInter.GetComponent<InterField>().ResetInterField();
        leftInter.SetActive(false);
        rightInter.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = true;
        canBattle = false;
        attackChance = false;
    }

    private void OnEnable()
    {
        FieldManager.instance.additionalCount--;
    }

    private void OnDisable()
    {
        FieldManager.instance.additionalCount++;
    }
}
