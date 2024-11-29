using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager
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
        EXP += Get_EXP();
        ATK += Next_ATK();
        HP += Next_HP();

        // 현재 레벨에서 얻는 경험치가 레벨업에 필요한 경험치보다 높으면
        if (EXP >= Max_EXP())
        {
            Level++;
            Main_UI.instance.TextCheck();
        }

        // 모든 플레이어 캐릭터의 공격력, 체력을 갱신
        for (int i = 0; i < Spawner.m_Players.Count; i++)
            Spawner.m_Players[i].Set_ATKHP();
    }

    // 경험치 퍼센트
    public float EXP_Percentage()
    {
        float exp = (float)Max_EXP();
        double myEXP = EXP;

        if (Level >= 1)
        {
            // 이전 레벨의 exp 만큼 빼주기 -> 다음 레벨이 되었을 때 0%부터 시작하기 위함
            exp -= (float)Max_EXP();
            myEXP -= (float)Max_EXP();
        }
        return (float) myEXP / exp;
    }

    // 레벨업 할 때마다 증가하는 경험치
    public float Next_EXP()
    {
        // 다음 레벨에 필요한 경험치
        float exp = (float)Max_EXP();
        float myExp = (float)Get_EXP();

        if (Level >= 1)
        {
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
        }
        return (myExp / exp) * 100.0f;
    }

    public double Get_EXP()
    {
        return Utils.CalculatedValue(Utils.Data.levelData.B_EXP, Level, Utils.Data.levelData.C_EXP);
    }

    public double Max_EXP()
    {
        return Utils.CalculatedValue(Utils.Data.levelData.B_MAXEXP, Level, Utils.Data.levelData.C_MAXEXP);
    }

    // 레벨업 할때마다 증가하는 ATK
    public double Next_ATK()
    {
        return Utils.CalculatedValue(Utils.Data.levelData.B_ATK, Level, Utils.Data.levelData.C_ATK);
    }

    // 레벨업 할때마다 증가하는 HP
    public double Next_HP()
    {
        return Utils.CalculatedValue(Utils.Data.levelData.B_HP, Level, Utils.Data.levelData.C_HP);
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
