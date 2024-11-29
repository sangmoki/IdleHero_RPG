using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 커스텀 에디터를 통해서 LevelDesign에 할당하지 않아도
// LevelDesign 스크립트를 사용할 수 있게 해준다.
[CustomEditor(typeof(LevelDesign))]
public class LevelDesignEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 만들어놓은 LevelDesign 스크립트를 가져온다.
        LevelDesign design = (LevelDesign)target;

        // 레벨 디자인을 위한 레이블을 만든다.
        EditorGUILayout.LabelField("Level Design", EditorStyles.boldLabel);

        LevelData data = design.levelData;

        EditorGUILayout.LabelField("캐릭터 레벨 그래프", EditorStyles.boldLabel);
        DrawGraph(data);
        EditorGUILayout.Space();

        // 레벨 데이터의 길이만큼 반복한다.
        DrawDefaultInspector();
    }

    private void DrawGraph(LevelData data)
    {
        EditorGUILayout.Space(20);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_ATK, data.currentLevel, data.C_ATK)), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_HP, data.currentLevel, data.C_HP)), Color.red);
        GetColorGUI("EXP", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_EXP, data.currentLevel, data.C_EXP)), Color.blue);
        GetColorGUI("MAXEXP", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_MAXEXP, data.currentLevel, data.C_MAXEXP)), Color.white);
        GetColorGUI("MONEY", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_MONEY, data.currentLevel, data.C_MONEY)), Color.yellow);
        EditorGUILayout.Space(20);
    }

    void GetColorGUI(string baseTemp, string dataTemp, Color color)
    {
        GUIStyle colorLabel = new GUIStyle(EditorStyles.label);
        colorLabel.normal.textColor = color;

        EditorGUILayout.LabelField(baseTemp + " : " + dataTemp, colorLabel);
    }
}
