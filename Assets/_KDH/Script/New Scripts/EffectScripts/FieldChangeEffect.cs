using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

[CreateAssetMenu(fileName = "FieldChangeEffect", menuName = "Effect/BaseEffect/FieldChangeEffect")]
public class FieldChangeEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObject caster, List<FieldCardObject> targets)
    {
        await Task.Delay(1);
        return;
    }
}
