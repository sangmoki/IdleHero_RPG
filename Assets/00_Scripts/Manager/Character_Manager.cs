using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager
{
    // 현재 착용중인 용병 정보
    public Character_Holder[] m_Set_Character = new Character_Holder[6];

    public void GetCharacter(int value, string character_Name)
    {
        // 착용중인 용병 정보를 변경
        m_Set_Character[value] = Base_Manager.Data.m_Data_Character[character_Name];
    }
}
