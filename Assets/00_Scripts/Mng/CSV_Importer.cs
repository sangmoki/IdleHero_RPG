using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSV_Importer : MonoBehaviour
{
    // CSV ������ �о�ͼ� ������ ����Ʈ
    public static List<Dictionary<string, object>> EXP = new List<Dictionary<string, object>>(CSVReader.Read("EXP"));
}
