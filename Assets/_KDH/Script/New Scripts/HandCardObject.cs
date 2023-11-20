using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using CCGCard;

[RequireComponent(typeof(BoxCollider2D))]
public class HandCardObject : MonoBehaviour
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
                transform.position = originPos;
                isDrag = false;
                isEmpty = true;
                ResetRender();
                gameObject.SetActive(false);
            }
            else if (cardData != value && value != null && value != new Card())
            {
                _cardData = value;
                gameObject.SetActive(true);
                isEmpty = false;
                RenderCard();
            }
        }
    }

    [Header("Show Card Data")]
    [SerializeField] Card _cardData;
    [Header("Input SpriteGO")]
    public GameObject spriteGO;
    private SpriteRenderer cardRenderer;
    [Header("Input Card Sprite")]
    public SpriteRenderer cardSprite;
    public Animator animator;
    [Header("Input Attack Texts")]
    public TMP_Text frontATKText;
    public TMP_Text backATKText;
    [Header("Input Name and Explain Text")]
    public TMP_Text nameText;
    public TMP_Text explainText;
    [Header("Show Is Empty")]
    public bool isEmpty = true;
    [Header("Input Gem And Rank")]
    public SpriteRenderer gemSprite;
    public TMP_Text rankText;
    [Header("Sound")]
    public AudioClip ClickSound;

    private BoxCollider2D boxCollider;
    private Vector3 originPos;
    private bool isDrag;
    private Vector3 originScale;

    /// <summary>
    /// ??類ㅼ굥??繞????쳛???濡ル츎 ?袁⑤?獄?
    /// </summary>
    public void CancelDrag()
    {
        PlayerActionManager.instance.isDrag = false;
        PlayerActionManager.instance.dragCardGO = null;
        transform.position = originPos;
        spriteGO.transform.localScale = originScale;
        frontATKText.enabled = true;
        backATKText.enabled = true;
        nameText.enabled = true;
        explainText.enabled = true;
        gemSprite.enabled = true;
        rankText.enabled = true;
        cardRenderer.color = new Color(1, 1, 1, 1);
        isDrag = false;
        boxCollider.enabled = true;
    }

    public void OpenDirection()
    {
        PlayerActionManager.instance.isDrag = false;
        transform.position = originPos;
        spriteGO.transform.localScale = originScale;
        frontATKText.enabled = true;
        backATKText.enabled = true;
        nameText.enabled = true;
        explainText.enabled = true;
        gemSprite.enabled = true;
        rankText.enabled = true;
        cardRenderer.color = new Color(1, 1, 1, 1);
        isDrag = false;
        boxCollider.enabled = true;
    }

    private void Start()
    {
        originPos = transform.position;
        originScale = spriteGO.transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
        cardRenderer = spriteGO.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.useCard == false && GameManager.instance.canAct && PlayerActionManager.instance.isDrag && isDrag && UIManager.Instance.isPopUI == false)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));
            transform.position = mousePos;
        }
    }

    private void OnMouseEnter()
    {

        if (GameManager.instance.useCard == false && PlayerActionManager.instance.isDrag == false && UIManager.Instance.isPopUI == false)
        {
            spriteGO.transform.localScale = 1.3f * originScale;
            spriteGO.transform.position -= new Vector3(0, 0, 0.5f);
        }
    }

    private void OnMouseExit()
    {
        if (GameManager.instance.useCard == false && PlayerActionManager.instance.isDrag == false && UIManager.Instance.isPopUI == false)
        {
            spriteGO.transform.localScale = originScale;
            spriteGO.transform.position += new Vector3(0, 0, 0.5f);
        }
    }

    private void OnMouseDown()
    {
        //Please Input Card Click Sound Code
        SoundManager.instance.PlayEffSound(ClickSound);
        if (GameManager.instance.gamePhase == GamePhase.ActionPhase && GameManager.instance.useCard == false && GameManager.instance.canAct && UIManager.Instance.isPopUI == false && FieldManager.instance.isOpenDirection == false)
        {
            isDrag = true;
            cardRenderer.color = new Color(cardRenderer.color.r, cardRenderer.color.g, cardRenderer.color.b, 0);
            frontATKText.enabled = false;
            backATKText.enabled = false;
            nameText.enabled = false;
            explainText.enabled = false;
            gemSprite.enabled = false;
            rankText.enabled = false;
            PlayerActionManager.instance.isDrag = true;
            PlayerActionManager.instance.dragCardGO = this;
            boxCollider.enabled = false;
        }
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.useCard == false && isDrag && PlayerActionManager.instance.field != null && PlayerActionManager.instance.field.isEmpty && UIManager.Instance.isPopUI == false && FieldManager.instance.isOpenDirection == false)
        {
            if (FieldManager.instance.battleField.Find(PlayerActionManager.instance.field.gameObject) == null)
            {
                if (PlayerActionManager.instance.field.Next == null)
                {
                    PlayerActionManager.instance.field.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    FieldManager.instance.make = true;
                    FieldManager.instance.makeLeft = false;

                    //FieldManagerTest.instance.battleField.AddAfter(PlayerActionManager.instance.field.Prev, PlayerActionManager.instance.field.gameObject);
                }
                else
                {
                    PlayerActionManager.instance.field.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    FieldManager.instance.make = true;
                    FieldManager.instance.makeLeft = true;
                    //FieldManagerTest.instance.battleField.AddBefore(PlayerActionManager.instance.field.Next, PlayerActionManager.instance.field.gameObject);
                }
            }
            PlayerActionManager.instance.selectUI.SetActive(true);
            FieldManager.instance.isOpenDirection = true;
            OpenDirection();
            PlayerActionManager.instance.CancelSelect += FieldManager.instance.CancelSelect;
            PlayerActionManager.instance.selectUI.transform.position = PlayerActionManager.instance.field.transform.position;
        }
        else if (FieldManager.instance.isOpenDirection == false)
        {
            CancelDrag();
        }
    }

    private void RenderCard()
    {
        cardSprite.sprite = cardData.cardSprite;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(cardData.animator);
        frontATKText.text = cardData.frontDamage.ToString();
        backATKText.text = cardData.backDamage.ToString();
        nameText.text = cardData.cardName;
        explainText.text = cardData.skillContents;
        if (cardData.skill != string.Empty)
        {
            switch (cardData.skill[0].ToString())
            {
                case "1":
                    gemSprite.color = Color.red;
                    break;
                case "2":
                    gemSprite.color = Color.green;
                    break;
                case "3":
                    gemSprite.color = Color.yellow;
                    break;
                case "4":
                    gemSprite.color = Color.cyan;
                    break;
                case "5":
                    gemSprite.color = Color.white;
                    break;
            }
            switch (cardData.skill[1].ToString())
            {
                case "1":
                    rankText.text = "I";
                    break;
                case "2":
                    rankText.text = "II";
                    break;
                case "3":
                    rankText.text = "III";
                    break;
                case "4":
                    rankText.text = "IV";
                    break;
                case "5":
                    rankText.text = "V";
                    break;
            }
        }
        else
        {
            gemSprite.color = Color.black;
            rankText.text = string.Empty;
        }
    }

    private void ResetRender()
    {
        animator.runtimeAnimatorController = null;
        cardSprite.sprite = null;
        frontATKText.text = string.Empty;
        backATKText.text = string.Empty;
        nameText.text = string.Empty;
        explainText.text = string.Empty;
        gemSprite.color = Color.black;
        rankText.text = string.Empty;
    }
}
