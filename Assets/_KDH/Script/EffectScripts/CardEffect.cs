using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    Summon,
    AttackStart,
    FindEnemy,
    CalculatePower,
    Attack,
    GetDamaged,
}

public abstract class CardEffect : ScriptableObject
{
    public abstract void ExecuteEffect(LinkedBattleField battleField, Field caster, List<Field> targets);
}
