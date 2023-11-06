using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardDB))]
public class CardDBEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardDB myDB = (CardDB)target;

        if(GUILayout.Button("Resource 폴더로부터 업데이트"))
        {
            myDB.LoadDataAll();
        }
    }
}
#endif