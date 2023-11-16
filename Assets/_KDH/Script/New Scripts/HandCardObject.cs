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

    [Header("移대뱶 ?뺣낫")]
    [SerializeField] Card _cardData;
    [Header("移대뱶 ?꾩껜 ? ?ㅽ봽?쇱씠??")]
    public GameObject spriteGO;
    private SpriteRenderer cardRenderer;
    [Header("?ㅼ젣 移대뱶???ㅽ봽?쇱씠??")]
    public SpriteRenderer cardSprite;
    public Animator animator;
    [Header("移대뱶??怨듦꺽???쒖떆??")]
    public TMP_Text frontATKText;
    public TMP_Text backATKText;
    [Header("移대뱶???대쫫怨??ㅻ챸??")]
    public TMP_Text nameText;
    public TMP_Text explainText;
    [Header("移대뱶媛 鍮꾩뼱?덈뒗吏 泥댄겕?섎뒗 蹂??")]
    public bool isEmpty = true;
    [Header("Sound")]
    private AudioClip ClickSound;

    private BoxCollider2D boxCollider;
    private Vector3 originPos;
    private bool isDrag;
    private Vector3 originScale;

    /// <summary>
    /// ?쒕옒洹?以?痍⑥냼?섎뒗 肄붾뱶
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
        }
    }

    private void OnMouseExit()
    {
        if (GameManager.instance.useCard == false && PlayerActionManager.instance.isDrag == false && UIManager.Instance.isPopUI == false)
        {
            spriteGO.transform.localScale = originScale;
        }
    }

    private void OnMouseDown()
    {
        //Please Input Card Click Sound Code
        //移대뱶 ?대┃ ???뚮━ ?ъ깮?섎뒗 肄붾뱶 ?ｌ뼱二쇱꽭??
        //SoundManager.instance.PlayEffSound(ClickSound);
        if (GameManager.instance.gamePhase == GamePhase.ActionPhase && GameManager.instance.useCard == false && GameManager.instance.canAct && UIManager.Instance.isPopUI == false && FieldManager.instance.isOpenDirection == false)
        {
            isDrag = true;
            cardRenderer.color = new Color(cardRenderer.color.r, cardRenderer.color.g, cardRenderer.color.b, 0);
            frontATKText.enabled = false;
            backATKText.enabled = false;
            nameText.enabled = false;
            explainText.enabled = false;
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
    }

    private void ResetRender()
    {
        animator.runtimeAnimatorController = null;
        cardSprite.sprite = null;
        frontATKText.text = string.Empty;
        backATKText.text = string.Empty;
        nameText.text = string.Empty;
        explainText.text = string.Empty;
    }
}
