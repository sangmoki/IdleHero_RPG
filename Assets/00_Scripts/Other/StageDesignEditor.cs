using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageDesign))]
public class StageDesignEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 만들어놓은 StageDesign 스크립트를 가져온다.
        StageDesign design = (StageDesign)target;

        // 레벨 디자인을 위한 레이블을 만든다.
        EditorGUILayout.LabelField("Stage Design", EditorStyles.boldLabel);

        StageData data = design.stageData;

        EditorGUILayout.Space(20);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_ATK, data.currentStage, data.M_ATK)), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_HP, data.currentStage, data.M_HP)), Color.red);
        GetColorGUI("MONEY", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_MONEY, data.currentStage, data.M_MONEY)), Color.blue);
        EditorGUILayout.Space(20);

        // 레벨 데이터의 길이만큼 반복한다.
        DrawDefaultInspector();
    }

    void GetColorGUI(string baseTemp, string dataTemp, Color color)
    {
        GUIStyle colorLabel = new GUIStyle(EditorStyles.label);
        colorLabel.normal.textColor = color;

        EditorGUILayout.LabelField(baseTemp + " : " + dataTemp, colorLabel);
    }
}
