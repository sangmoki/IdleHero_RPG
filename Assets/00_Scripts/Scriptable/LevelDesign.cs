using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Level Design/Level Design Data")]
public class LevelDesign : ScriptableObject
{
    public int currentLevel;
    public int currentStage;

    public LevelData levelData;
    [Space(20)]
    public StageData stageData;
}

[System.Serializable]
public class LevelData
{
    [Range(0.0f, 10.0f)]
    public float C_ATK, C_HP, C_EXP, C_MAXEXP, C_MONEY; // 현재 레벨의 공격력, 체력, 경험치, 최대 경험치, 돈

    [Space(20)]
    [Header("## Base Value")]
    public float B_ATK;
    public float B_HP;
    public float B_EXP;
    public float B_MAXEXP;
    public float B_MONEY;

    public double ATK() => Utils.CalculatedValue(B_ATK, Data_Manager.m_data.Level, C_ATK);
    public double HP() => Utils.CalculatedValue(B_HP, Data_Manager.m_data.Level, C_HP);
    public double EXP() => Utils.CalculatedValue(B_EXP, Data_Manager.m_data.Level, C_EXP);
    public double MAXEXP() => Utils.CalculatedValue(B_MAXEXP, Data_Manager.m_data.Level, C_MAXEXP);
    public double MONEY() => Utils.CalculatedValue(B_MONEY, Data_Manager.m_data.Level, C_MONEY);
}

[System.Serializable]
public class StageData
{
    [Range(0.0f, 10.0f)]
    public float M_ATK, M_HP, M_MONEY;
    [Space(20f)]
    [Header("## Base Value")]
    public float B_ATK;
    public float B_HP;
    public float B_MONEY;

    public double ATK() => Utils.CalculatedValue(B_ATK, Data_Manager.m_data.Stage, M_ATK);
    public double HP() => Utils.CalculatedValue(B_HP, Data_Manager.m_data.Stage, M_HP);
    public double MONEY() => Utils.CalculatedValue(B_MONEY, Data_Manager.m_data.Stage, M_MONEY);

}

