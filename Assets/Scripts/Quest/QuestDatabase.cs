using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName= "Quest/QuestDatabase")]
public class QuestDatabase : ScriptableObject
{
    [SerializeField] private List<Quest> _quests;

    public IReadOnlyList<Quest> Quests => _quests;

    public Quest FindQuestBy(string ID) => _quests.FirstOrDefault(x => x.Id == ID);
    //지정된 조건을 만족하는 요소가 없다면 컬렉션 데이터의 기본 값을 반환하는 기능
    //LINQ에서 사용되는 경우 조건을 만족하는 요소가 존재하는 지를 체크하는 용도로
    //사용합니다.
}
