using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Level Design/Level Design Data")]
public class LevelDesign : ScriptableObject
{
    public LevelData levelData;

}

[System.Serializable]
public class LevelData
{
    public int currentLevel;        // 현재 레벨
    public float C_ATK, C_HP, C_EXP, C_MAXEXP, C_MONEY; // 현재 레벨의 공격력, 체력, 경험치, 최대 경험치, 돈
}
