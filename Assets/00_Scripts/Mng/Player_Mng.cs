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
        // ���� �������� ��� ����ġ
        EXP += float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());

        // ���� �������� ��� ����ġ�� �������� �ʿ��� ����ġ���� ������
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
            // ���� ������ exp ��ŭ ���ֱ� -> ���� ������ �Ǿ��� �� 0%���� �����ϱ� ����
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
            myEXP -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
        }
        return (float) myEXP / exp;
    }

    public float Next_EXP()
    {
        // ���� ������ �ʿ��� ����ġ
        float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
        float myExp = float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());

        if (Level >= 1)
        {
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
        }
        return (myExp / exp) * 100.0f;
    }

    // ������ �Ҷ����� �����ϴ� ATK
    public double Next_ATK()
    {
        return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 5;
    }

    // ������ �Ҷ����� �����ϴ� HP
    public double Next_HP()
    {
        return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 3;
    }
}
