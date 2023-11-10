using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CalculateDamageEffect", menuName = "Effect/BaseEffect/CalculateDamageEffect")]
public class CalculateDamageEffect : CardEffect
{
    public override async void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        Debug.Log(targets.Count);
        if (caster.canBattle)
        {
            System.Threading.Tasks.Task atkTask = GameManager.Instance.CheckAnim(caster.animator, "Attack");

            caster.animator.Play("Attack");
            await atkTask;
            caster.canBattle = false;
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
                        System.Threading.Tasks.Task deathTask = GameManager.Instance.CheckAnim(caster.animator, "Death");
                        await deathTask;
                    }
                    else
                    {
                        Debug.LogError("아이템을 공격할 수 없습니다");
                    }
                }
                else
                {
                    Debug.Log(caster.unitObject.cardData.cardName + "의 공격이 실패했습니다.");

                    Debug.Log("HitStart");
                    System.Threading.Tasks.Task hitTask = GameManager.Instance.CheckAnim(targets[i].animator, "Hit");
                    targets[i].animator.Play("Hit");
                    Debug.Log("HitAnimation");
                    await hitTask;
                    Debug.Log("HitEnd");
                    //targets[i].unitObject.lookingLeft = !targets[i].unitObject.lookingLeft;
                    if (targets[i].animator.runtimeAnimatorController != null)
                    {
                        if (targets[i].canBattle)
                        {
                            Debug.Log("Hit Can Battle");
                            targets[i].animator.Play("Idle");
                        }
                        else
                        {
                            Debug.Log("Hit Can't Battle");
                            targets[i].animator.Play("Breath");
                        }
                    }
                }
            }
        }
    }
}
