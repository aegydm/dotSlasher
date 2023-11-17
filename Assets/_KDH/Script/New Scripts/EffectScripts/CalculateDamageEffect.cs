using CCGCard;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CalculateDamageEffect", menuName = "Effect/BaseEffect/CalculateDamageEffect")]
public class CalculateDamageEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObject caster, List<FieldCardObject> targets)
    {
        Debug.Log(targets.Count);
        if (caster.attackChance)
        {
            Task atkTask = GameManager.instance.CheckAnim(caster.animator, "Attack");

            caster.animator.Play("Attack");
            await atkTask;
            caster.attackChance = false;
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
                        GameManager.instance.enemyDamageSum += attackPower;
                        if (targets[i].lookingLeft == caster.lookingLeft)
                        {
                            targets[i].lookingLeft = !targets[i].lookingLeft;
                        }
                    }
                    else if (targets[i].cardData.cardCategory == CardCategory.minion)
                    {
                        targets[i].cardData.GetDamage(targets[i], attackPower);
                        Task deathTask = GameManager.instance.CheckAnim(caster.animator, "Death");
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
                    if (targets[i].lookingLeft == caster.lookingLeft)
                    {
                        targets[i].lookingLeft = !targets[i].lookingLeft;
                    }
                    Task hitTask = GameManager.instance.CheckAnim(targets[i].animator, "Hit");
                    targets[i].animator.Play("Hit");
                    Debug.Log("HitAnimation");
                    await hitTask;
                    Debug.Log("HitEnd");
                    if (targets[i].animator.runtimeAnimatorController != null)
                    {
                        if (targets[i].attackChance)
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
            caster.canBattle = false;
        }
        return;
    }
}
