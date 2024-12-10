using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Object/Character", order = int.MaxValue)]
public class Character_Scriptable : ScriptableObject
{
    public string m_Character_Name;
    public float m_Attack_Range;
    public Rarity m_Rarity;
    public int MaxMP;
}
