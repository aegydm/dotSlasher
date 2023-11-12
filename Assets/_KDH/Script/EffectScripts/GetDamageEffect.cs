using CCGCard;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageEffect", menuName = "Effect/BaseEffect/GetDamageEffect")]
public class GetDamageEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        caster.animator.Play("Death");
        Task deathTask = GameManager.Instance.CheckAnim(caster.animator, "Death");
        await deathTask;
        caster.unitObject.CardChange(new Card());
        caster.ResetField();
        return;
    }
}
