using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;
using UnityEditor;

[CreateAssetMenu]
public class CardDB : ScriptableObject
{
    public static CardDB instance
    {
        get
        {
            if (_instance == null)
            {
                CardDB[] asset = Resources.LoadAll<CardDB>("");
                if (asset == null || asset.Length < 1)
                {
                    throw new Exception($"Could not find any singleton scriptable object instance in the resources.");
                }
                else if (asset.Length > 1)
                {
                    Debug.LogWarning("Multiple Instance of the singleton scriptable object found in resource.");
                }
                _instance = asset[0];
            }

            return _instance;
        }
        private set
        {
            _instance = new CardDB();
        }
    }

    private static CardDB _instance;

    public List<Card> cards;

    private CardDB() { }

    public Card FindCardFromID(int id)
    {
        foreach (var data in cards)
        {
            if (data.cardID == id)
            {
                if (data.cardCategory != CardCategory.hero)
                {
                    Card outCard = data.Copy();
                    return outCard;
                }
                else
                {
                    Card outCard = Hero.ChangeToHero(data);
                    Debug.Log(outCard is Hero);
                    return outCard;
                }
            }
        }
        return null;
    }

    public void CheckHero()
    {

        foreach (var data in cards)
        {
            Debug.Log(data is Hero);
        }
    }

    public void LoadDataAll()
    {
        ReadCharData("CardDataBase", cards);
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    private void ReadCharData(string v, List<Card> cards)
    {
        cards.Clear();
        List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
        dicList.Clear();
        dicList = CSVReader.Read(v);
        for (int i = 0; i < dicList.Count; i++)
        {
            Card card;
            if ((CardCategory)Enum.Parse(typeof(CardCategory), dicList[i]["type"].ToString()) == CardCategory.hero)
            {
                card = new Hero();
                card.getDamageEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Hero/HeroGetDamageEffect"));
            }
            else
            {
                card = new Card();
                card.getDamageEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/GetDamageEffect"));
            }
            card.cardID = int.Parse(dicList[i]["ID"].ToString());
            card.cardName = dicList[i]["name"].ToString();
            card.cardColor = (CardType)Enum.Parse(typeof(CardType), dicList[i]["faction"].ToString());
            card.cardCategory = (CardCategory)Enum.Parse(typeof(CardCategory), dicList[i]["type"].ToString());
            card.frontDamage = int.Parse(dicList[i]["F_Atk"].ToString());
            card.backDamage = int.Parse(dicList[i]["B_Atk"].ToString());
            card.animator = ("Sprites/" + dicList[i]["faction"].ToString() + "/" + dicList[i]["faction"].ToString() + "_" + dicList[i]["type"].ToString() + "/" + dicList[i]["sprite"].ToString() + "/" + dicList[i]["sprite"].ToString());
            card.cardSprite = Resources.Load<Sprite>("Sprites/" + dicList[i]["faction"].ToString() + "/" + dicList[i]["faction"].ToString() + "_" + dicList[i]["type"].ToString() + "/" + dicList[i]["sprite"].ToString() + "/" + dicList[i]["sprite"].ToString());
            card.skill = dicList[i]["skill"].ToString();
            card.skillContents = dicList[i]["skill"].ToString() + "\n" + dicList[i]["skillContents"].ToString();
            card.summonEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/SummonEffect"));
            card.fieldChangeEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/FieldChangeEffect"));
            card.attackStartEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/AttackStartEffect"));
            card.findEnemyEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/FindEnemyEffect"));
            card.calculateDamageEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/CalculateDamageEffect"));
            card.attackProcessEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Base/AttackProcessEffect"));
            if(card.skill == "결속") 
            {
                card.fieldChangeEffects.Clear();
                card.fieldChangeEffects.Add(Resources.Load<CardEffect>("ScriptableObject/Effects/FieldAttackIncrease"));
            }
            cards.Add(card);
        }

    }
}
