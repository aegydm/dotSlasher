using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ���� �ʱ�ȭ�� �����ϱ� ���� ������
/// </summary>
public abstract class InitialSuccessValue : ScriptableObject
{
    public abstract int GetValue(Task task);
}
