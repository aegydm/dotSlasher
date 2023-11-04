using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageEffect", menuName = "Effect/BaseEffect/GetDamageEffect")]
public class GetDamageEffect : CardEffect
{
    public override void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        Debug.Log("GETDAMAGE");
    }
}
