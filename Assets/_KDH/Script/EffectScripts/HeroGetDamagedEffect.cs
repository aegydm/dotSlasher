using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroGetDamageEffect", menuName = "Effect/HeroEffect/HeroGetDamageEffect")]
public class HeroGetDamagedEffect : GetDamageEffect
{
    public override async void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        caster.animator.Play("Hit");
        System.Threading.Tasks.Task hitTask = GameManager.Instance.CheckAnim(caster.animator, "Hit");
        await hitTask;
        caster.animator.Play("Breath");
    }
}
