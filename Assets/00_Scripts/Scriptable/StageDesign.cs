using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDesignData", menuName = "Stage Design/Stage Design Data")]

public class StageDesign : ScriptableObject
{
    public StageData stageData;
}

[System.Serializable]
public class StageData
{
    public int currentStage;        // 현재 레벨
    [Range(0.0f, 10.0f)]
    public float M_ATK, M_HP, M_MONEY; // 현재 레벨의 공격력, 체력, 경험치, 최대 경험치, 돈

    [Space(20f)]
    [Header("## Base Value")]
    public float B_ATK;
    public float B_HP;
    public float B_MONEY;
}
