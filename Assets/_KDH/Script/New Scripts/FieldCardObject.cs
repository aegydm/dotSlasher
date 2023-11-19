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

    [Header("For show Card Data")]
    [SerializeField] private Card _cardData;
    [Header("For check is empty")]
    public bool isEmpty = true;
    [Header("Input FIeld Sprite")]
    [SerializeField] SpriteRenderer fieldSprite;
    [Header("Input Unit Sprite")]
    public SpriteRenderer cardSprite;
    public Animator animator;
    [Header("Input Front, Back Attack TMP_TEXT")]
    public TMP_Text frontATKText;
    public TMP_Text backATKText;
    [Header("Input Gem Image and Rank Text")]
    public Image gemImage;
    public TMP_Text rankText;

    public GameObject gemAddGO;
    public GameObject gemMultiGO;
    public GameObject rankAddGO;
    public GameObject rankMultiGO;

    public TMP_Text gemAddText;
    public TMP_Text gemMultiText;
    public TMP_Text rankAddText;
    public TMP_Text rankMultiText;

    [Header("Sound")]
    public AudioClip ClickSound;

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
    [Header("For Check looking left")]
    [SerializeField] private bool _lookingLeft = false;
    [Header("For Check Player ID")]
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
                ownerColor.color = new Color(255 - (255 * playerID), 255 * playerID, 0, 1);
            }
            else
            {
                ownerColor.color = Color.white;
                ownerColor.color = new Color(ownerColor.color.r, ownerColor.color.g, ownerColor.color.b, 0);
            }
        }
    }
    [Header("For show Owner Color")]
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

    [Header("For show Can Battle")]
    [SerializeField] bool _canBattle = false;
    [Header("For show Has Attack Chance")]
    [SerializeField] private bool _attackChance;
    [Header("Input Battle Image?")]
    public Image canBattleImage;
    [Header("Input Left InterField")]
    public GameObject leftInter;
    [Header("Input Right InterField")]
    public GameObject rightInter;
    [Header("For Show Prev and Next Tile")]
    public FieldCardObject Prev;
    public FieldCardObject Next;

    public void RenderCard()
    {
        cardSprite.sprite = cardData.cardSprite;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(cardData.animator);
        frontATKText.text = _lookingLeft ? (cardData.backDamage).ToString() : (cardData.frontDamage).ToString();
        backATKText.text = _lookingLeft ? (cardData.frontDamage).ToString() : (cardData.backDamage).ToString();
        if (cardData.skill != string.Empty)
        {
            switch (cardData.skill[0].ToString())
            {
                case "1":
                    gemImage.color = Color.red;
                    break;
                case "2":
                    gemImage.color = Color.green;
                    break;
                case "3":
                    gemImage.color = Color.yellow;
                    break;
                case "4":
                    gemImage.color = Color.cyan;
                    break;
                case "5":
                    gemImage.color = Color.white;
                    break;
            }
            switch (cardData.skill[1].ToString())
            {
                case "1":
                    rankText.text = "1";
                    break;
                case "2":
                    rankText.text = "2";
                    break;
                case "3":
                    rankText.text = "3";
                    break;
                case "4":
                    rankText.text = "4";
                    break;
                case "5":
                    rankText.text = "5";
                    break;
            }
        }
        else
        {
            gemImage.color = Color.black;
            rankText.text = string.Empty;
        }
        if (cardData != null)
        {
            if (cardData.frontDamage != CardDB.instance.FindCardFromID(cardData.cardID).frontDamage)
            {
                if (!lookingLeft)
                {

                    frontATKText.color = Color.yellow;
                }
                else
                {
                    backATKText.color = Color.yellow;
                }
            }
            else
            {
                if (cardData.frontDamage != 0)
                {
                    //frontATKText.color = Color.white;
                    //backATKText.color = Color.white;
                    gemAddText.text = string.Empty;
                    gemAddGO.SetActive(false);
                    gemMultiText.text = string.Empty;
                    gemMultiGO.SetActive(false);
                    rankAddText.text = string.Empty;
                    rankAddGO.SetActive(false);
                    rankMultiText.text = string.Empty;
                    rankMultiGO.SetActive(false);
                }
                else
                {
                    if (!lookingLeft)
                    {
                        frontATKText.color = Color.white;
                    }
                    else
                    {
                        backATKText.color = Color.white;
                    }
                }
            }
            if (cardData.backDamage != CardDB.instance.FindCardFromID(cardData.cardID).backDamage)
            {
                if (!lookingLeft)
                {
                    backATKText.color = Color.yellow;
                }
                else
                {
                    frontATKText.color = Color.yellow;
                }
            }
            else
            {
                if (cardData.backDamage != 0)
                {
                    //frontATKText.color = Color.white;
                    //backATKText.color = Color.white;
                    gemAddText.text = string.Empty;
                    gemAddGO.SetActive(false);
                    gemMultiText.text = string.Empty;
                    gemMultiGO.SetActive(false);
                    rankAddText.text = string.Empty;
                    rankAddGO.SetActive(false);
                    rankMultiText.text = string.Empty;
                    rankMultiGO.SetActive(false);
                }
                else
                {
                    if (!lookingLeft)
                    {
                        backATKText.color = Color.white;
                    }
                    else
                    {
                        frontATKText.color = Color.white;
                    }
                }
            }
        }
        else
        {
            gemAddText.text = string.Empty;
            gemAddGO.SetActive(false);
            gemMultiText.text = string.Empty;
            gemMultiGO.SetActive(false);
            rankAddText.text = string.Empty;
            rankAddGO.SetActive(false);
            rankMultiText.text = string.Empty;
            rankMultiGO.SetActive(false);
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
        gemAddText.text = string.Empty;
        gemAddGO.SetActive(false);
        gemMultiText.text = string.Empty;
        gemMultiGO.SetActive(false);
        rankAddText.text = string.Empty;
        rankAddGO.SetActive(false);
        rankMultiText.text = string.Empty;
        rankMultiGO.SetActive(false);
        gemImage.color = Color.black;
        rankText.text = string.Empty;
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
        SoundManager.instance.PlayEffSound(ClickSound);
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
        //Debug.LogError("DelayCanAttackFalse");
        GameManager.instance.canAct = false;
        //Debug.LogError("DelayIsAttackFalse");
        GameManager.instance.isAlreadyAttack = false;
        //Debug.LogError("DelayEnd");
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
        if (GameManager.instance != null && FieldManager.instance != null && GameManager.instance.isStart)
        {
            FieldManager.instance.additionalCount--;
        }
    }

    private void OnDisable()
    {
        if (GameManager.instance != null && FieldManager.instance != null && GameManager.instance.isStart)
        {
            FieldManager.instance.additionalCount++;
        }
    }
}
