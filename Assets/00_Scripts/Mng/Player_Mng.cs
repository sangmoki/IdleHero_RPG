using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mng
{
    public int Level;                        // 레벨
    public double EXP;                       // 경험치  
    public double ATK = 10;                  // 공격력
    public double HP = 50;                   // 체력

    public float Critical_Percent = 20.0f;   // 크리티컬 확률
    public double Critical_Damage = 140.0d; // 크리티컬 데미지

    // 경험치 증가 시 이벤트
    public void EXP_UP()
    {
        // 경험치, 공격력, 체력 증가
        EXP += float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());
        ATK += Next_ATK();
        HP += Next_HP();

        // 현재 레벨에서 얻는 경험치가 레벨업에 필요한 경험치보다 높으면
        if (EXP >= float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString()))
        {
            Level++;
            Main_UI.instance.TextCheck();
        }

        // 모든 플레이어 캐릭터의 공격력, 체력을 갱신
        for (int i = 0; i < Spawner.m_Players.Count; i++)
            Spawner.m_Players[i].Set_ATKHP();
    }

    public float EXP_Percentage()
    {
        float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
        double myEXP = EXP;

        if (Level >= 1)
        {
            // 이전 레벨의 exp 만큼 빼주기 -> 다음 레벨이 되었을 때 0%부터 시작하기 위함
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
            myEXP -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
        }
        return (float) myEXP / exp;
    }

    public float Next_EXP()
    {
        // 다음 레벨에 필요한 경험치
        float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
        float myExp = float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());

        if (Level >= 1)
        {
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
        }
        return (myExp / exp) * 100.0f;
    }

    // 레벨업 할때마다 증가하는 ATK
    public double Next_ATK()
    {
        return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 5;
    }

    // 레벨업 할때마다 증가하는 HP
    public double Next_HP()
    {
        return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 3;
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
