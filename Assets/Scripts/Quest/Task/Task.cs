using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 퀘스트에서의 작업 내역을 만들기 위한 가장 기본적인 틀
/// 프로그램 내에서 만들어줄 데이터(Scriptable Object)
/// Scriptable Object : 유니티 내에서 연결해서 사용하는 것(Monobehaviour)
/// 라면 ScriptableObject를 연결 시 에디터 내에서 해당 형태의 클래스로 설계된
/// 데이터를 오브젝트처럼 생성해서 사용할 수 있습니다.
/// </summary>

public enum State
{
    Inactive, Running, Complete
}



[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    [Header("ID")] //작업을 분류하는 기준 (아이디 코드 / 작업의 설명)
    [SerializeField] private string id;
    [SerializeField] private string description;

    [Header("Settings")] //작업에 필요한 기본 설정 (성공 횟수)
    [SerializeField] private int s_count;
    [SerializeField] private InitialSuccessValue initialSuccessValue;
    [SerializeField] private bool RecieveValue;

    [Header("Action")] //작업에 대한 Action
    [SerializeField] private TaskAction action;

    [Header("Target")] //작업에 대한 Target
    [SerializeField] private TaskTarget[] targets;

    [Header("Category")] //작업 카테고리
    [SerializeField] private Category category;

    private State state; //작업 상태
    private int current_success; //현재의 수치

    #region 상태의 변화에 따라 처리할 이벤트 설계(delegate)
    public delegate void OnStateChanged(Task task, State current_state, State pre_state);
    //대리자
    public delegate void OnCompleted(Task task, int current_success, int pre_success);

    #endregion

    public event OnStateChanged onStateChanged; //상태 변화 시 처리할 이벤트
    public event OnCompleted onCompleted;       //퀘스트 완료 시 처리할 이벤트

    #region 프로퍼티
    //작업 성공 자체에 대한 프로퍼티(읽기 전용)
    //설명, 코드, 성공 횟수에 대한 접근 프로퍼티
    public int Success { get; private set; } //private set이 붙는 경우 읽기 전용
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

// ? 연산자는 Null이 아니라면 참조하고, NULL이면 Null로써 처리하게 하는 연산자 기호입니다.

//?? 연산자는 Null이라면 오른쪽 값으로 처리하세요.

//delegate : 대리자를 의미하며, 메소드에 대한 포인터를 저장하며,
//주로 이벤트에 대한 처리를 진행할 때 사용됩니다.

//Invoke : 별도의 스레드에서 하나의 컨트롤 개체에 접근하려 할 때
//서로 다른 스레드가 하나의 컨트롤 개체에 접근하는 것을 방지합니다.
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
    /// 성공 횟수를 전달해 Success 프로퍼티에 적용합니다.
    /// </summary>
    /// <param name="s_count">성공 횟수</param>
    public void Receive(int s_count)
    {
        Success = action.Run(this, Success, s_count);
    }

    /// <summary>
    /// LINQ(쿼리 언어) 형태로 작업에 대한 요청 설계
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsTarget(string category, object target)
        => 
        Category == Category &&
        targets.Any(x => x.IsTarget(target)) &&
        (!IsComplete || (IsComplete && RecieveValue));
    //묶음의 형태에서 작업할 수 있는 LINQ Any 기능
    //  해당 타겟들 중에서 값이 타겟과 동일한지를
    // 체크합니다.
    //Any : 하나라도 맞으면 true
    //All : 전체 조사

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
