using CCGCard;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroGetDamageEffect", menuName = "Effect/HeroEffect/HeroGetDamageEffect")]
public class HeroGetDamagedEffect : GetDamageEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        caster.animator.Play("Hit");
        Task hitTask = GameManager.Instance.CheckAnim(caster.animator, "Hit");
        await hitTask;
        caster.animator.Play("Breath");
        return;
    }
}
