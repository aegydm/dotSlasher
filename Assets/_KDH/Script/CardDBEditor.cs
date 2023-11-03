using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardDB))]
public class CardDBEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardDB myDB = (CardDB)target;

        if(GUILayout.Button("Resource �����κ��� ������Ʈ"))
        {
            myDB.LoadDataAll();
        }
    }
}
