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

    public override bool Init()
    {
        Main_UI.instance.FadeInOut(true, true, null);

        var datas = Base_Manager.Data.m_Data_Character;

        // 가져온 용병 정보를 반복을 하며 딕셔너리에 저장
        foreach(var data in datas)
        { 
            // 딕셔너리에 데이터 저장 (키값을 이름으로 Value는 데이터)
            m_Dictionarys.Add(data.Value.data.m_Character_Name, data.Value.data);
        }

        // 딕셔너리 정렬
        var sort_dictionary = m_Dictionarys.OrderByDescending(x => x.Value.m_Rarity);

        // 반복 돌며 레어도에 따른 정렬
        foreach (var data in sort_dictionary)
        {
            // Content를 부모 오브젝트로 Part(Prefabs화 한 Hero_Panel)를 생성한다.
            var go = Instantiate(Part, Content).GetComponent<UI_Heroes_Part>();
            go.Initalize(data.Value);
        }

        return base.Init();
    }

    public override void DisableOBJ()
    {
        // FadeIn이 진행되고 난 후 FadeOut을 진행한다.
        Main_UI.instance.FadeInOut(false, true, () =>
        {
            Main_UI.instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });
    }
}
