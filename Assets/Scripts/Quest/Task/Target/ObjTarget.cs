using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/GameObject", fileName = "Go_Target_")]
public class ObjTarget : TaskTarget
{
    [SerializeField] GameObject value;

    public override object Value => value;

    /// <summary>
    /// 게임 오브젝트의 경우 clone의 형태로 만들어지는 경우가 많다보니 Equal 보다는
    /// 해당 이름이 포함되어있는지 정도를 체크하는게 더 자연스럽게 구현이 가능할 것 같습니다.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public override bool IsTarget(object target)
    {
        GameObject targetObj = target as GameObject;
        if (targetObj != null)
        {
            return targetObj.name.Contains(value.name);
        }
        return false;
    }
}

