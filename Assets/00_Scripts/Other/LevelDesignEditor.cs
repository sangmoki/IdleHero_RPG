using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 커스텀 에디터를 통해서 LevelDesign에 할당하지 않아도
// LevelDesign 스크립트를 사용할 수 있게 해준다.
[CustomEditor(typeof(LevelDesign))]
public class LevelDesignEditor : Editor
{
    LevelDesign design = null;

    public override void OnInspectorGUI()
    {
        // 만들어놓은 LevelDesign 스크립트를 가져온다.
        design = (LevelDesign)target;

        // 레벨 디자인을 위한 레이블을 만든다.
        EditorGUILayout.LabelField("Level Design", EditorStyles.boldLabel);

        LevelData data = design.levelData;
        StageData s_data = design.stageData;

        DrawGraph(data, s_data);
        EditorGUILayout.Space();

        // 레벨 데이터의 길이만큼 반복한다.
        DrawDefaultInspector();
    }

    private void DrawGraph(LevelData data, StageData s_Data)
    {
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("레벨 데이터", EditorStyles.boldLabel);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.Data.levelData.ATK()), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.Data.levelData.HP()), Color.red);
        GetColorGUI("EXP", StringMethod.ToCurrencyString(Utils.Data.levelData.EXP()), Color.blue);
        GetColorGUI("MAXEXP", StringMethod.ToCurrencyString(Utils.Data.levelData.MAXEXP()), Color.white);
        GetColorGUI("MONEY", StringMethod.ToCurrencyString(Utils.Data.levelData.MONEY()), Color.yellow);
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("스테이지 데이터", EditorStyles.boldLabel);
        GetColorGUI("M_ATK", StringMethod.ToCurrencyString(Utils.Data.stageData.ATK()), Color.green);
        GetColorGUI("M_HP", StringMethod.ToCurrencyString(Utils.Data.stageData.HP()), Color.red);
        GetColorGUI("M_MONEY", StringMethod.ToCurrencyString(Utils.Data.stageData.MONEY()), Color.blue);
    }

    void GetColorGUI(string baseTemp, string dataTemp, Color color)
    {
        GUIStyle colorLabel = new GUIStyle(EditorStyles.label);
        colorLabel.normal.textColor = color;

        EditorGUILayout.LabelField(baseTemp + " : " + dataTemp, colorLabel);
    }
}
