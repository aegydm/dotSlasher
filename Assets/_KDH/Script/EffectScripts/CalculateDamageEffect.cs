using CCGCard;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CalculateDamageEffect", menuName = "Effect/BaseEffect/CalculateDamageEffect")]
public class CalculateDamageEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObjectTest caster, List<FieldCardObjectTest> targets)
    {
        Debug.Log(targets.Count);
        if (caster.canBattle)
        {
            Task atkTask = TestManager.instance.CheckAnim(caster.animator, "Attack");

            caster.animator.Play("Attack");
            await atkTask;
            caster.canBattle = false;
            caster.animator.Play("Breath");
            for (int i = 0; i < targets.Count; i++)
            {
                int attackPower = caster.cardData.frontDamage;
                int defencePower = caster.lookingLeft != targets[i].lookingLeft ? targets[i].cardData.frontDamage : targets[i].cardData.backDamage;
                Debug.Log(caster.cardData.cardName + "의 공격력 : " + attackPower + "\n" + targets[i].cardData.cardName + "의 공격력 : " + defencePower);

                if (attackPower > defencePower)
                {
                    Debug.Log(caster.cardData.cardName + "의 공격이 성공했습니다.");
                    //targets[i].Damaged(attackPower);
                    if (targets[i].cardData.cardCategory == CardCategory.hero)
                    {
                        targets[i].cardData.GetDamage(targets[i], attackPower);
                    }
                    else if (targets[i].cardData.cardCategory == CardCategory.minion)
                    {
                        targets[i].cardData.GetDamage(targets[i], attackPower);
                        Task deathTask = TestManager.instance.CheckAnim(caster.animator, "Death");
                        await deathTask;
                    }
                    else
                    {
                        Debug.LogError("아이템을 공격할 수 없습니다");
                    }
                }
                else
                {
                    Debug.Log(caster.cardData.cardName + "의 공격이 실패했습니다.");
                    Debug.Log("HitStart");
                    Task hitTask = TestManager.instance.CheckAnim(targets[i].animator, "Hit");
                    targets[i].animator.Play("Hit");
                    Debug.Log("HitAnimation");
                    await hitTask;
                    Debug.Log("HitEnd");
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
        return;
    }
}
