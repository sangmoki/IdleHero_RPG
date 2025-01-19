using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager
{
    public double ATK = 10;                  // 공격력
    public double HP = 50;                   // 체력

    public float Critical_Percent = 20.0f;   // 크리티컬 확률
    public double Critical_Damage = 140.0d; // 크리티컬 데미지

    // 경험치 증가 시 이벤트
    public void EXP_UP()
    {
        // 경험치, 공격력, 체력 증가
        Data_Manager.m_data.EXP += Utils.Data.levelData.EXP();
        ATK += Utils.Data.levelData.ATK();
        HP += Utils.Data.levelData.HP();

        // 현재 레벨에서 얻는 경험치가 레벨업에 필요한 경험치보다 높으면
        if (Data_Manager.m_data.EXP >= Utils.Data.levelData.MAXEXP())
        {
            Data_Manager.m_data.Level++;
            Data_Manager.m_data.EXP = 0;
            Main_UI.instance.TextCheck();
        }

        // 모든 플레이어 캐릭터의 공격력, 체력을 갱신
        for (int i = 0; i < Spawner.m_Players.Count; i++)
            Spawner.m_Players[i].Set_ATKHP();
    }

    // 경험치 퍼센트
    public float EXP_Percentage()
    {
        float exp = (float)Utils.Data.levelData.MAXEXP();
        double myEXP = Data_Manager.m_data.EXP;

        return (float) myEXP / exp;
    }

    // 레벨업 할 때마다 증가하는 경험치
    public float Next_EXP()
    {
        // 다음 레벨에 필요한 경험치
        float exp = (float)Utils.Data.levelData.MAXEXP();
        float myExp = (float)Utils.Data.levelData.EXP();

        return (myExp / exp) * 100.0f;
    }

    // 공격력 = 공격력 * 아이템 등급
    public double Get_ATK(Rarity rarity)
    {
        return ATK * ((int)rarity + 1);
    }

    // HP = 체력 * 아이템 등급
    public double Get_HP(Rarity rarity)
    {
        return HP * ((int) rarity + 1);
    }

    // DPS = 공격력 + 체력
    public double Average_DPS()
    {
        return ATK + HP;
    }
}
