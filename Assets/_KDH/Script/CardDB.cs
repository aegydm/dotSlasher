using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;

[CreateAssetMenu]
public class CardDB : ScriptableObject
{
    public List<Card> cards;

    public void LoadDataAll()
    {
        ReadCharData("CardDataBase", cards);
    }

    private void ReadCharData(string v, List<Card> cards)
    {
        cards.Clear();
        List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
        dicList.Clear();
        dicList = CSVReader.Read(v);
        for (int i = 0; i < dicList.Count; i++)
        {
            Card card = new Card();
            card.cardName = dicList[i]["name"].ToString();
            card.cardColor = (CardType)Enum.Parse(typeof(CardType), dicList[i]["faction"].ToString());
            card.cardCategory = (CardCategory)Enum.Parse(typeof(CardCategory), dicList[i]["type"].ToString());
            card.frontDamage = int.Parse(dicList[i]["F_Atk"].ToString());
            card.backDamage = int.Parse(dicList[i]["B_Atk"].ToString());
            card.cardSprite = Resources.Load<Sprite>("Sprites/" + dicList[i]["faction"].ToString() + "/" + dicList[i]["sprite"].ToString());
            card.summonEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/SummonEffect"));
            card.attackStartEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/AttackStartEffect"));
            card.findEnemyEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/FindEnemyEffect"));
            card.calculateDamageEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/CalculateDamageEffect"));
            card.attackProcessEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/AttackProcessEffect"));
            card.getDamageEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/GetDamageEffect"));
            cards.Add(card);
        }
    }
}
