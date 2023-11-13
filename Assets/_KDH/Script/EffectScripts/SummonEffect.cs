using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonEffect", menuName = "Effect/BaseEffect/SummonEffect")]
public class SummonEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObjectTest caster, List<FieldCardObjectTest> targets)
    {
        await Task.Delay((int)(Time.deltaTime*1000));
        return;
    }
}
