using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 성공 값에 대한 초기화를 진행하기 위한 데이터
/// </summary>
public abstract class InitialSuccessValue : ScriptableObject
{
    public abstract int GetValue(Task task);
}
