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

        EditorGUILayout.LabelField("캐릭터 레벨 그래프", EditorStyles.boldLabel);
        DrawGraph(data);
        EditorGUILayout.Space();

        // 레벨 데이터의 길이만큼 반복한다.
        DrawDefaultInspector();
    }

    private void DrawGraph(LevelData data)
    {
        Rect rect = GUILayoutUtility.GetRect(200, 100);
        Handles.DrawSolidRectangleWithOutline(rect, Color.black, Color.white);
        
        Vector3[] curvePoint_ATK = GraphDesign(rect, data.C_ATK);
        Handles.color = Color.green;
        Handles.DrawAAPolyLine(3, curvePoint_ATK);

        Vector3[] curvePoint_HP = GraphDesign(rect, data.C_HP);
        Handles.color = Color.red;
        Handles.DrawAAPolyLine(3, curvePoint_HP);

        Vector3[] curvePoint_EXP = GraphDesign(rect, data.C_EXP);
        Handles.color = Color.blue;
        Handles.DrawAAPolyLine(3, curvePoint_EXP);

        Vector3[] curvePoint_MAXEXP = GraphDesign(rect, data.C_MAXEXP);
        Handles.color = Color.white;
        Handles.DrawAAPolyLine(3, curvePoint_MAXEXP);

        Vector3[] curvePoint_MONEY = GraphDesign(rect, data.C_MONEY);
        Handles.color = Color.yellow;
        Handles.DrawAAPolyLine(3, curvePoint_MONEY);

        EditorGUILayout.Space(20);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(design.CalculatedValue(10, data.currentLevel, data.C_ATK)), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(design.CalculatedValue(50, data.currentLevel, data.C_HP)), Color.red);
        GetColorGUI("EXP", StringMethod.ToCurrencyString(design.CalculatedValue(5, data.currentLevel, data.C_EXP)), Color.blue);
        GetColorGUI("MAXEXP", StringMethod.ToCurrencyString(design.CalculatedValue(15, data.currentLevel, data.C_MAXEXP)), Color.white);
        GetColorGUI("MONEY", StringMethod.ToCurrencyString(design.CalculatedValue(10, data.currentLevel, data.C_MONEY)), Color.yellow);
        EditorGUILayout.Space(20);
    }

    void GetColorGUI(string baseTemp, string dataTemp, Color color)
    {
        GUIStyle colorLabel = new GUIStyle(EditorStyles.label);
        colorLabel.normal.textColor = color;

        EditorGUILayout.LabelField(baseTemp + " : " + dataTemp, colorLabel);
    }

    private Vector3[] GraphDesign(Rect rect, float data)
    {
        Vector3[] curvePoint = new Vector3[100];
        for (int i = 0; i < 100; i++)
        {
            float t = i / 99.0f;
            float curveValue = Mathf.Pow(t, data);
            curvePoint[i] = new Vector3(
                rect.x + t * rect.width,
                rect.y + rect.height - curveValue + rect.height, 
                0);
        }
        return curvePoint;

    }
}
