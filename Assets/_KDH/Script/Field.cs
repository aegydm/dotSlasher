using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

public class Field : MonoBehaviour
{
    public bool isEmpty = true;
    public Card card;
    public SpriteRenderer spriteRenderer;
    public Field prevField;
    public Field nextField;
}
