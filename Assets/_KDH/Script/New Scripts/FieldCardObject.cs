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

    [Header("?꾨뱶???ㅼ뼱?덈뒗 移대뱶???뺣낫")]
    [SerializeField] private Card _cardData;
    [Header("?꾨뱶媛 鍮꾩뼱?덈뒗吏 泥댄겕?섎뒗 蹂??")]
    public bool isEmpty = true;
    [Header("?꾨뱶 ?꾩껜 ? ?ㅽ봽?쇱씠??")]
    [SerializeField] SpriteRenderer fieldSprite;
    [Header("?ㅼ젣 移대뱶???ㅽ봽?쇱씠??")]
    public SpriteRenderer cardSprite;
    public Animator animator;
    [Header("移대뱶??怨듦꺽???쒖떆??")]
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
    [Header("移대뱶??醫뚯슦")]
    [SerializeField] private bool _lookingLeft = false;
    [Header("?꾨뱶 ?뚯쑀?먯쓽 ID")]
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
    [Header("移대뱶???뚯쑀???쒖떆???대?吏")]
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

    [Header("?대떦 ?꾨뱶媛 怨듦꺽???????덈뒗吏 ?щ?")]
    [SerializeField] bool _canBattle = false;
    [Header("?대떦 ?꾨뱶媛 怨듦꺽沅뚯쓣 媛吏怨??덈뒗吏 ?щ?")]
    [SerializeField] private bool _attackChance;
    [Header("怨듦꺽 媛???щ? ?쒖떆???대?吏")]
    public Image canBattleImage;
    [Header("醫뚯륫 ?쇱뼱?ㅺ린 ??- First???뚮쭔 ?쒖꽦???덉젙")]
    public GameObject leftInter;
    [Header("?곗륫 ?쇱뼱?ㅺ린 媛???곸뿭 ?쒖떆??")]
    public GameObject rightInter;
    [Header("?댁쟾 移멸낵 ?ㅼ쓬 移몄쓽 ?뺣낫")]
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
        //移대뱶 ?대┃ ?ъ슫??肄붾뱶 ?ｌ뼱二쇱꽭??
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
}
