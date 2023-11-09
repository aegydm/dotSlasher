using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageEffect", menuName = "Effect/BaseEffect/GetDamageEffect")]
public class GetDamageEffect : CardEffect
{
    public override async void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        caster.animator.Play("Death");
        System.Threading.Tasks.Task deathTask = GameManager.Instance.CheckAnim(caster.animator, "Death");
        await deathTask;
        caster.unitObject.CardChange(new Card());
        caster.ResetField();
    }
}
