using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "AttackProcessEffect",menuName = "Effect/BaseEffect/AttackProcessEffect")]
public class AttackProcessEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObjectTest caster, List<FieldCardObjectTest> targets)
    {
        await Task.Delay((int)(Time.deltaTime * 1000));
        return;
    }
}
