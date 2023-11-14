using CCGCard;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageEffect", menuName = "Effect/BaseEffect/GetDamageEffect")]
public class GetDamageEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObject caster, List<FieldCardObject> targets)
    {
        caster.animator.Play("Death");
        Task deathTask = GameManager.instance.CheckAnim(caster.animator, "Death");
        await deathTask;
        if(caster.playerID.ToString() == GameManager.instance.playerID)
        {
            GameManager.instance.deck.grave.Add(caster.cardData);
            GameManager.instance.deck.RefreshGraveCount();
        }
        else
        {
            GameManager.instance.deck.enemyGrave.Add(caster.cardData);
            GameManager.instance.deck.RefreshEnemyGraveCount();
        }
        caster.cardData = null;
        return;
    }
}
