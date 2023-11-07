using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ÿ�� ����
/// </summary>
[CreateAssetMenu(menuName= "Category", fileName = "Category_")]
public class Category : ScriptableObject , IEquatable<Category>
{
    [SerializeField] private string id;
    [SerializeField] private string ingame_name;
    public string ID => id;
    public string Ingame_Name => ingame_name;

    public bool Equals(Category other)
    {
        //�ٸ� ���� ����ִٸ� false
        if(other is null) return false;
        //���۷����� ������ ��, ����� ���� ��ġ�Ѵٸ� true
        if(ReferenceEquals(other,this)) return true;
        //������ ���°� �ٸ� ��쿡�� false
        if(GetType() != other.GetType()) return false;        
        //�Ϲ����� ��쿡�� id�� �������� ���� ���θ� üũ�մϴ�.
        return id == other.id;
    }

    public override int GetHashCode() => (id, ingame_name).GetHashCode();
    public override bool Equals(object obj) => base.Equals(obj);

    //Ŭ���� ���� �� �����ڿ� ���� ����(������ �����ε�)
    public static bool operator ==(Category category, string id)
    {
        if (category is null)
            return ReferenceEquals(id, null);
        return category.id == id || category.ingame_name == id;
    }

    public static bool operator !=(Category category, string id) => !(category == id);
  
}
