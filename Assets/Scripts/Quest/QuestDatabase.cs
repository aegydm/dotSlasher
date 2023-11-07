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
    //������ ������ �����ϴ� ��Ұ� ���ٸ� �÷��� �������� �⺻ ���� ��ȯ�ϴ� ���
    //LINQ���� ���Ǵ� ��� ������ �����ϴ� ��Ұ� �����ϴ� ���� üũ�ϴ� �뵵��
    //����մϴ�.
}
