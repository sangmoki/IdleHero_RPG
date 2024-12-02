using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Manager
{
    Dictionary<string, Item_Scriptable> Item_Datas = new Dictionary<string, Item_Scriptable>();

    public void Init()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");

        for (int i = 0; i < datas.Length; i++)
        {
            // 데이터의 이름을 키로 사용하여 datas 배열에 저장
            // CSV파일을 읽어오기 위한 키값 ITEM_001과 같은
            Item_Datas.Add(datas[i].name, datas[i]);
            Debug.Log(datas[i].name + " : " + datas[i].Item_Name);
        }
    }

    // 아이템 드랍 확률
    public List<Item_Scriptable> GetDropSet()
    {
        List<Item_Scriptable> objs = new List<Item_Scriptable>();

        // Dictionary 데이터를 반복을 통해 데이터를 가져옴
        foreach (var data in Item_Datas)
        {
            // 0 ~ 100 사이의 랜덤한 값 생성
            float valueCount = Random.Range(0.0f, 100.0f);
            // 랜덤한 값이 아이템 드랍확률보다 낮거나 같으면 아이템을 추가
            if (valueCount <= data.Value.Item_Chance)
            {
                objs.Add(data.Value);
            }
        }
        return objs;
    }
}
