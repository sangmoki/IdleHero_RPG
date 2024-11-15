using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mng
{
    public int Level;
    public double EXP;
    public double ATK = 10;
    public double HP = 50;

    public void EXP_UP()
    {
        // 현재 레벨에서 얻는 경험치
        EXP += float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());

        // 현재 레벨에서 얻는 경험치가 레벨업에 필요한 경험치보다 높으면
        if (EXP >= float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString()))
        {
            Level++;
        }
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
}
