using CCGCard;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageEffect", menuName = "Effect/BaseEffect/GetDamageEffect")]
public class GetDamageEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObjectTest caster, List<FieldCardObjectTest> targets)
    {
        caster.animator.Play("Death");
        Task deathTask = TestManager.instance.CheckAnim(caster.animator, "Death");
        await deathTask;
        if(caster.playerID.ToString() == TestManager.instance.playerID)
        {
            TestManager.instance.deck.grave.Add(caster.cardData);
        }
        else
        {
            TestManager.instance.deck.enemyGrave.Add(caster.cardData);
        }
        caster.cardData = null;
        return;
    }
}
