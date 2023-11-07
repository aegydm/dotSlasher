using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 추상 클래스를 상속받을 경우, 해당 클래스가 가지고 있는
/// 선언되어있는 메소드(추상 메소드)에 대한 구현을 진행합니다.(override : 재정의)
/// </summary>

[CreateAssetMenu(menuName = "Quest/Task/Action/OneCountAction", fileName ="One Count_")]
public class OneCountAction : TaskAction
{
    /// <summary>
    /// 현 작업과 성공 횟수를 더한 값을 내보냅니다.
    /// </summary>
    /// <param name="task">태스크</param>
    /// <param name="success">현재의 성공 상태</param>
    /// <param name="s_count">성공 횟수</param>
    /// <returns></returns>
    public override int Run(Task task, int success, int s_count)
    {
        return success + s_count;
    }
}

