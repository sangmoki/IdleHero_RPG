using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DB로 활용할 클래스

// 동료 용병 정보
public class Character_Holder
{
    public Character_Scriptable data;   // 용병 데이터
    public int Level;           // 용병 레벨
    public int Count;           // 용병 조각 개수
}

// 유저 캐릭터 정보
public class Data_Manager
{
    public double Money;        // 현재 골드
    public int Level;           // 현재 레벨
    public double EXP;          // 경험치  
    public int Stage;           // 현재 스테이지

    // 플레이어가 가지고 있는 용병 정보
    public Dictionary<string, Character_Holder> m_Data_Character = new Dictionary<string, Character_Holder>();

    public void Init()
    {
        Set_Character();
    }

    private void Set_Character()
    {
        // Scriptable Object에서 데이터를 가져옴
        var datas = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        // 반복을 통해 Dictionary에 data를 초기화 해 넣어준다.
        foreach (var data in datas)
        {
            var character = new Character_Holder();

            // 용병 초기 정보
            character.data = data;
            character.Level = 0;
            character.Count = 0;

            m_Data_Character.Add(data.m_Character_Name, character);
        }
    }
}
