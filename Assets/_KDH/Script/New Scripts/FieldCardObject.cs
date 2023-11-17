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

    [Header("?袁⑤굡????쇰선??덈뮉 燁삳?諭???類ｋ궖")]
    [SerializeField] private Card _cardData;
    [Header("?袁⑤굡揶쎛 ??쑴堉??덈뮉筌왖 筌ｋ똾寃??롫뮉 癰궰??")]
    public bool isEmpty = true;
    [Header("?袁⑤굡 ?袁⑷퍥 ?? ??쎈늄??깆뵠??")]
    [SerializeField] SpriteRenderer fieldSprite;
    [Header("??쇱젫 燁삳?諭????쎈늄??깆뵠??")]
    public SpriteRenderer cardSprite;
    public Animator animator;
    [Header("燁삳?諭???⑤벀爰????뽯뻻??")]
    public TMP_Text frontATKText;
    public TMP_Text backATKText;

    [Header("Sound")]
    private AudioClip ClickSound;
    public void CheckInter()
    {
        if (FieldManager.instance.FieldIsFull() == false)
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
    [Header("燁삳?諭???ル슣??")]
    [SerializeField] private bool _lookingLeft = false;
    [Header("?袁⑤굡 ?????癒?벥 ID")]
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
    [Header("燁삳?諭??????????뽯뻻?????筌왖")]
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

    [Header("?????袁⑤굡揶쎛 ?⑤벀爰????????덈뮉筌왖 ???")]
    [SerializeField] bool _canBattle = false;
    [Header("?????袁⑤굡揶쎛 ?⑤벀爰썸쾮??뱽 揶쎛筌왖????덈뮉筌왖 ???")]
    [SerializeField] private bool _attackChance;
    [Header("?⑤벀爰?揶쎛????? ??뽯뻻?????筌왖")]
    public Image canBattleImage;
    [Header("?ル슣瑜???깅선??븍┛ ??- First?????춸 ??뽮쉐????됱젟")]
    public GameObject leftInter;
    [Header("?怨쀫? ??깅선??븍┛ 揶쎛???怨몃열 ??뽯뻻??")]
    public GameObject rightInter;
    [Header("??곸읈 燁삳㈇????쇱벉 燁삳챷???類ｋ궖")]
    public FieldCardObject Prev;
    public FieldCardObject Next;

    public void RenderCard()
    {
        cardSprite.sprite = cardData.cardSprite;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(cardData.animator);
        frontATKText.text = _lookingLeft ? (cardData.backDamage).ToString() : (cardData.frontDamage).ToString();
        backATKText.text = _lookingLeft ? (cardData.frontDamage).ToString() : (cardData.backDamage).ToString();
    }

    private void ResetRender()
    {
        animator.runtimeAnimatorController = null;
        cardSprite.sprite = null;
        frontATKText.text = string.Empty;
        backATKText.text = string.Empty;
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
        //燁삳?諭???????????꾨뗀諭??節뚮선雅뚯눘苑??
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
