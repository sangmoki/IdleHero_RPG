using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Level Design/Level Design Data")]
public class LevelDesign : ScriptableObject
{
    public LevelData levelData;

    // 지수 증가 공식
    // 값이 일정한 비율로 지속적으로 증가한다.
    // 레벨이 높아질 수록 추후에 증가할 값도 높아진다.
    // baseValue : 기본값
    // Level : 레벨
    // value : 지수
    public float CalculatedValue(float baseValue, int Level, float value)
    {
        // Mathf.Pow(Single, Single) 거듭제곱
        // 즉, baseValue * Level^value
        return baseValue * Mathf.Pow(Level, value);
    }
}

[System.Serializable]
public class LevelData
{
    public int currentLevel;        // 현재 레벨
    public float C_ATK, C_HP, C_EXP, C_MAXEXP, C_MONEY; // 현재 레벨의 공격력, 체력, 경험치, 최대 경험치, 돈
}
