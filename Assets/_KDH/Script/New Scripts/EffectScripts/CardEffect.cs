using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public abstract Task ExecuteEffect(LinkedBattleField battleField, FieldCardObject caster, List<FieldCardObject> targets);
}
