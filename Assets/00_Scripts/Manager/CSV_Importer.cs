using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSV_Importer
{
    // 레벨업 시 스폰 정보
    public static List<Dictionary<string, object>> Spawn_Design = new List<Dictionary<string, object>>(CSVReader.Read("Spawner"));
    
}
