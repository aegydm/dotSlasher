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

    [Header("카드 정보")]
    [SerializeField] Card _cardData;
    [Header("카드 전체 틀 스프라이트")]
    public GameObject spriteGO;
    private SpriteRenderer cardRenderer;
    [Header("실제 카드의 스프라이트")]
    public SpriteRenderer cardSprite;
    public Animator animator;
    [Header("카드의 공격력 표시용")]
    public TMP_Text frontATKText;
    public TMP_Text backATKText;
    [Header("카드가 비어있는지 체크하는 변수")]
    public bool isEmpty = true;

    private BoxCollider2D boxCollider;
    private Vector3 originPos;
    private bool isDrag;

    /// <summary>
    /// 드래그 중 취소하는 코드
    /// </summary>
    public void CancelDrag()
    {
        PlayerActionManager.instance.isDrag = false;
        PlayerActionManager.instance.dragCardGO = null;
        transform.position = originPos;
        spriteGO.transform.localScale = new(1, 1, 1);
        frontATKText.enabled = true;
        backATKText.enabled = true;
        cardRenderer.color = new Color(1, 1, 1, 1);
        isDrag = false;
        boxCollider.enabled = true;
    }

    private void Start()
    {
        originPos = transform.position;
        boxCollider = GetComponent<BoxCollider2D>();
        cardRenderer = spriteGO.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (PlayerActionManager.instance.isDrag && isDrag)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));
            transform.position = mousePos;
        }
    }

    private void OnMouseEnter()
    {

        if (PlayerActionManager.instance.isDrag == false)
        {
            spriteGO.transform.localScale = new(1.3f, 1.3f, 1.3f);
        }
    }

    private void OnMouseExit()
    {

        if (PlayerActionManager.instance.isDrag == false)
        {
            spriteGO.transform.localScale = new(1, 1, 1);
        }
    }

    private void OnMouseDown()
    {
        isDrag = true;
        cardRenderer.color = new Color(1, 1, 1, 0);
        frontATKText.enabled = false;
        backATKText.enabled=false;
        PlayerActionManager.instance.isDrag = true;
        PlayerActionManager.instance.dragCardGO = this;
        boxCollider.enabled = false;
    }

    private void OnMouseUp()
    {
        if (isDrag && PlayerActionManager.instance.field != null && PlayerActionManager.instance.field.isEmpty)
        {
            if(FieldManagerTest.instance.battleField.Find(PlayerActionManager.instance.field.gameObject) == null)
            {
                if(PlayerActionManager.instance.field.Next == null)
                {
                    Debug.Log("ADD1");
                    PlayerActionManager.instance.field.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    FieldManagerTest.instance.battleField.AddAfter(PlayerActionManager.instance.field.Prev, PlayerActionManager.instance.field.gameObject);
                }
                else
                {
                    Debug.Log("Add2");
                    PlayerActionManager.instance.field.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    FieldManagerTest.instance.battleField.AddBefore(PlayerActionManager.instance.field.Next, PlayerActionManager.instance.field.gameObject);

                }
            }
            PlayerActionManager.instance.field.cardData = cardData;
            PlayerActionManager.instance.CancelWithNewField();
            PlayerActionManager.instance.dirtyForInter = false;
            PlayerActionManager.instance.RemoveHandCard(this);
        }
        else
        {
            CancelDrag();
        }
        FieldManagerTest.instance.CheckInterAll();
    }

    private void RenderCard()
    {
        cardSprite.sprite = cardData.cardSprite;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(cardData.animator);
        frontATKText.text = cardData.frontDamage.ToString();
        backATKText.text = cardData.backDamage.ToString();
    }

    private void ResetRender()
    {
        animator.runtimeAnimatorController = null;
        cardSprite.sprite = null;
        frontATKText.text = string.Empty;
        backATKText.text = string.Empty;
    }
}
