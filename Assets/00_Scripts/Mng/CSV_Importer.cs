using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSV_Importer : MonoBehaviour
{
    // CSV 파일을 읽어와서 저장할 리스트
    public static List<Dictionary<string, object>> EXP = new List<Dictionary<string, object>>(CSVReader.Read("EXP"));
}
