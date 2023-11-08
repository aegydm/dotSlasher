using CCGCard;
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
            caster.animator.Play("Attack");
            while (caster.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Debug.Log(1);
                if (caster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    break;
                }
            }
            for (int i = 0; i < targets.Count; i++)
            {
                int attackPower = caster.unitObject.cardData.frontDamage;
                int defencePower = caster.unitObject.lookingLeft != targets[i].unitObject.lookingLeft ? targets[i].unitObject.cardData.frontDamage : targets[i].unitObject.cardData.backDamage;
                Debug.Log(caster.unitObject.cardData.cardName + "의 공격력 : " + attackPower + "\n" + targets[i].unitObject.cardData.cardName + "의 공격력 : " + defencePower);

                if (attackPower > defencePower)
                {
                    Debug.Log(caster.unitObject.cardData.cardName + "의 공격이 성공했습니다.");
                    //targets[i].Damaged(attackPower);
                    if (targets[i].unitObject.cardData.cardCategory == CardCategory.hero)
                    {
                        targets[i].unitObject.cardData.GetDamage(targets[i], ref attackPower);
                    }
                    else if (targets[i].unitObject.cardData.cardCategory == CardCategory.minion)
                    {
                        targets[i].unitObject.cardData.GetDamage(targets[i], ref attackPower);
                    }
                    else
                    {
                        Debug.LogError("아이템을 공격할 수 없습니다");
                    }
                }
                else
                {
                    Debug.Log(caster.unitObject.cardData.cardName + "의 공격이 실패했습니다.");
                    targets[i].animator.Play("Hit");
                    while (caster.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
                    {
                        if (caster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                        {
                            break;
                        }
                    }
                    if (targets[i].canBattle)
                    {
                        targets[i].animator.Play("Idle");
                    }
                    else
                    {
                        targets[i].animator.Play("Breath");
                    }
                }
            }
            caster.canBattle = false;
        }
    }
}
