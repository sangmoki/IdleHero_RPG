using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Data", menuName = "Item Data/Item")]

public class Item_Scriptable : ScriptableObject
{
    public string Item_Name;    // 아이템 이름
    public string Item_Des;     // 아이템 설명
    public Rarity rarity;       // 레어도
    public float Item_Chance;   // 아이템 드랍 확률
}
