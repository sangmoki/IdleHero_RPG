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
    public int C_ATK, C_HP, C_EXP, C_MAXEXP, C_MONEY; // 현재 레벨의 공격력, 체력, 경험치, 최대 경험치, 돈

    [Space(20)]
    [Header("## Base Value")]
    public int B_ATK;
    public int B_HP;
    public int B_EXP;
    public int B_MAXEXP;
    public int B_MONEY;
}

[System.Serializable]
public class StageData
{
    [Range(0.0f, 10.0f)]
    public int M_ATK, M_HP, M_MONEY;
    [Space(20f)]
    [Header("## Base Value")]
    public int B_ATK;
    public int B_HP;
    public int B_MONEY;
}

