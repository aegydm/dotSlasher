using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CalculateDamageEffect", menuName = "Effect/BaseEffect/CalculateDamageEffect")]
public class CalculateDamageEffect : CardEffect
{
    public override void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        Debug.Log(targets.Count);
        if (caster.canBattle)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                int attackPower = caster.unitObject.cardData.frontDamage;
                int defencePower = caster.unitObject.lookingLeft != targets[i].unitObject.lookingLeft ? targets[i].unitObject.cardData.frontDamage : targets[i].unitObject.cardData.backDamage;
                Debug.Log(caster.unitObject.cardData.cardName + "의 공격력 : " + attackPower + "\n" + targets[i].unitObject.cardData.cardName + "의 공격력 : " + defencePower);

                if (attackPower > defencePower)
                {
                    Debug.Log(caster.unitObject.cardData.cardName + "의 공격이 성공했습니다.");
                    targets[i].Damaged(attackPower);
                }
                else
                {
                    Debug.Log(caster.unitObject.cardData.cardName + "의 공격이 실패했습니다.");
                }
            }
            caster.canBattle = false;
        }
    }
}
