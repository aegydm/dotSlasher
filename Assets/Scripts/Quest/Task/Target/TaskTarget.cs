using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class TaskTarget : ScriptableObject
{
    /// <summary>
    /// 값에 대한 프로퍼티
    /// </summary>
    public abstract object Value { get; }

    /// <summary>
    /// 타겟이 Task에서 지정한 타겟인지를 확인하는 용도
    /// </summary>
    public abstract bool IsTarget(object target);

}

