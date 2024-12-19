using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Object/Character", order = int.MaxValue)]
public class Character_Scriptable : ScriptableObject
{
    public string m_Character_Name;     // 캐릭터 이름
    public float m_Attack_Range;        // 공격 범위
    public float m_Attack_Speed;        // 공격 속도
    public Rarity m_Rarity;             // 캐릭터 등급
    public int MaxMP;                   // 최대 마나
}
