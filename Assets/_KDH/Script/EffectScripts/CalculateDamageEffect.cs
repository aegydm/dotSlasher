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
                Debug.Log(caster.unitObject.cardData.cardName + "�� ���ݷ� : " + attackPower + "\n" + targets[i].unitObject.cardData.cardName + "�� ���ݷ� : " + defencePower);

                if (attackPower > defencePower)
                {
                    Debug.Log(caster.unitObject.cardData.cardName + "�� ������ �����߽��ϴ�.");
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
                        Debug.LogError("�������� ������ �� �����ϴ�");
                    }
                }
                else
                {
                    Debug.Log(caster.unitObject.cardData.cardName + "�� ������ �����߽��ϴ�.");
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
