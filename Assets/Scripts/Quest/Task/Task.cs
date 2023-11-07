using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// ����Ʈ������ �۾� ������ ����� ���� ���� �⺻���� Ʋ
/// ���α׷� ������ ������� ������(Scriptable Object)
/// Scriptable Object : ����Ƽ ������ �����ؼ� ����ϴ� ��(Monobehaviour)
/// ��� ScriptableObject�� ���� �� ������ ������ �ش� ������ Ŭ������ �����
/// �����͸� ������Ʈó�� �����ؼ� ����� �� �ֽ��ϴ�.
/// </summary>

public enum State
{
    Inactive, Running, Complete
}



[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    [Header("ID")] //�۾��� �з��ϴ� ���� (���̵� �ڵ� / �۾��� ����)
    [SerializeField] private string id;
    [SerializeField] private string description;

    [Header("Settings")] //�۾��� �ʿ��� �⺻ ���� (���� Ƚ��)
    [SerializeField] private int s_count;
    [SerializeField] private InitialSuccessValue initialSuccessValue;
    [SerializeField] private bool RecieveValue;

    [Header("Action")] //�۾��� ���� Action
    [SerializeField] private TaskAction action;

    [Header("Target")] //�۾��� ���� Target
    [SerializeField] private TaskTarget[] targets;

    [Header("Category")] //�۾� ī�װ�
    [SerializeField] private Category category;

    private State state; //�۾� ����
    private int current_success; //������ ��ġ

    #region ������ ��ȭ�� ���� ó���� �̺�Ʈ ����(delegate)
    public delegate void OnStateChanged(Task task, State current_state, State pre_state);
    //�븮��
    public delegate void OnCompleted(Task task, int current_success, int pre_success);

    #endregion

    public event OnStateChanged onStateChanged; //���� ��ȭ �� ó���� �̺�Ʈ
    public event OnCompleted onCompleted;       //����Ʈ �Ϸ� �� ó���� �̺�Ʈ

    #region ������Ƽ
    //�۾� ���� ��ü�� ���� ������Ƽ(�б� ����)
    //����, �ڵ�, ���� Ƚ���� ���� ���� ������Ƽ
    public int Success { get; private set; } //private set�� �ٴ� ��� �б� ����
    public string Description => description;
    public string Id => id;
    public int S_Count => s_count;

    public Category Category => category;

    public bool IsComplete => State == State.Complete;

    public Quest Owner { get; private set; }

    public State State
    {
        get => state;
        set
        {
            var pre_state = state;
            state = value;
            onStateChanged?.Invoke(this, pre_state, value);           

// ? �����ڴ� Null�� �ƴ϶�� �����ϰ�, NULL�̸� Null�ν� ó���ϰ� �ϴ� ������ ��ȣ�Դϴ�.

//?? �����ڴ� Null�̶�� ������ ������ ó���ϼ���.

//delegate : �븮�ڸ� �ǹ��ϸ�, �޼ҵ忡 ���� �����͸� �����ϸ�,
//�ַ� �̺�Ʈ�� ���� ó���� ������ �� ���˴ϴ�.

//Invoke : ������ �����忡�� �ϳ��� ��Ʈ�� ��ü�� �����Ϸ� �� ��
//���� �ٸ� �����尡 �ϳ��� ��Ʈ�� ��ü�� �����ϴ� ���� �����մϴ�.
        }
    }


    public int Current_Success
    {
        get => current_success;
        set
        {
            int pre_success = current_success;
            current_success = Mathf.Clamp(value,0,s_count);
            if(current_success != pre_success)
            {
                State = current_success == s_count ? State.Complete : State.Running;

                onCompleted?.Invoke(this, current_success, pre_success);
            }
        }
    }
    #endregion

    /// <summary>
    /// ���� Ƚ���� ������ Success ������Ƽ�� �����մϴ�.
    /// </summary>
    /// <param name="s_count">���� Ƚ��</param>
    public void Receive(int s_count)
    {
        Success = action.Run(this, Success, s_count);
    }

    /// <summary>
    /// LINQ(���� ���) ���·� �۾��� ���� ��û ����
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsTarget(string category, object target)
        => 
        Category == Category &&
        targets.Any(x => x.IsTarget(target)) &&
        (!IsComplete || (IsComplete && RecieveValue));
    //������ ���¿��� �۾��� �� �ִ� LINQ Any ���
    //  �ش� Ÿ�ٵ� �߿��� ���� Ÿ�ٰ� ����������
    // üũ�մϴ�.
    //Any : �ϳ��� ������ true
    //All : ��ü ����

    public void Start()
    {
        State = State.Running;
        if (initialSuccessValue)
            current_success = initialSuccessValue.GetValue(this);
    }

    public void End()
    {
        onStateChanged = null;
        onCompleted= null;
    }

    public void Complete()
    {
        Current_Success = S_Count;
    }

    public void Setup(Quest owner)
    {
        Owner = owner;
    }
}
