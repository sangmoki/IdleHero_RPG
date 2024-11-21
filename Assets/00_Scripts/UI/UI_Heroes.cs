using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Heroes : UI_Base
{
    public Transform Content;   // Hero_Panel의 부모 오브젝트
    public GameObject Part;     // Hero_Panel Prefabs

    // 정렬 위한 Dictionary
    Dictionary<string, Character_Scriptable> m_Dictionarys = new Dictionary<string, Character_Scriptable>();

    private void Start()
    {
        var Data = Resources.LoadAll<Character_Scriptable>("Scriptable");

        for (int i = 0; i < Data.Length; i++)
        {
            // 딕셔너리에 데이터 저장
            m_Dictionarys.Add(Data[i].m_Character_Name, Data[i]);

        }

        // 딕셔너리 정렬
        var sort_dictionary = m_Dictionarys.OrderByDescending(x => x.Value.m_Rarity);

        // 반복 돌며 레어도에 따른 정렬
        foreach(var data in sort_dictionary)
        {
            // Content를 부모 오브젝트로 Part(Prefabs화 한 Hero_Panel)를 생성한다.
            var go = Instantiate(Part, Content).GetComponent<UI_Heroes_Part>();
            go.Initalize(data.Value);
        }
    }
}
