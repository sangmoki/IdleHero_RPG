using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heroes : UI_Base
{
    public Transform Content;   // Hero_Panel의 부모 오브젝트
    public GameObject Part;     // Hero_Panel Prefabs
    public List<UI_Heroes_Part> parts = new List<UI_Heroes_Part>();

    // 정렬 위한 Dictionary
    Dictionary<string, Character_Scriptable> m_Dictionarys = new Dictionary<string, Character_Scriptable>();
    Character_Scriptable m_Character;

    // 영웅 탭 + 클릭 버튼 생성
    public void InitButtons()
    {
        for (int i = 0; i < Render_Manager.instance.HERO.Circles.Length; i++)
        {
            int index = i;
            // Button이라는 새 오브젝트를 생성하여 Button 컴포넌트 추가.
            var go = new GameObject("Button").AddComponent<Button>();
            // 클릭이벤트 구현
            go.onClick.AddListener(() => SetCharacterButton(index));
            // 부모 오브젝트를 Hero_Panel로 설정
            go.transform.SetParent(this.transform);
            // 버튼에 이미지와 RectTrasnform 컴포넌트 추가
            go.gameObject.AddComponent<Image>();
            go.gameObject.AddComponent<RectTransform>();

            // rect를 가져와 offset(Anchors presets) 설정
            RectTransform rect = go.GetComponent<RectTransform>();

            // 0.0에 위치하게 설정
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            rect.sizeDelta = new Vector2(150.0f, 150.0f);
            go.GetComponent<Image>().color = new Color(0, 0, 0, 0.01f);

            go.transform.position = Render_Manager.instance.ReturnScreenPoint(Render_Manager.instance.HERO.Circles[i]);
        }
    }

    // 어느 위치로 영웅을 배치할 것 인지 이벤트 구현
    private void SetCharacterButton(int value)
    {
        Base_Manager.Character.GetCharacter(value, m_Character.m_Character_Name);
        
        Render_Manager.instance.HERO.GetParticle(false);
        OnClickHero(null);
        Render_Manager.instance.HERO.InitHero();

        // 영웅을 착용중인지 확인
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].GetCharacterCheck();
        }
    }

    // 영웅 클릭 이벤트
    public void OnClickHero(UI_Heroes_Part s_Part)
    {
        if (s_Part == null)
        {
            // 모든 영웅에 Lock이라는 오브젝트를 활성화 하고 Outline을 비활성화.
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].LockOBJ.SetActive(false);
                parts[i].GetComponent<Outline>().enabled = false;
            }
        }
        else
        {
            m_Character = s_Part.m_Character;

            // 모든 영웅에 Lock이라는 오브젝트를 활성화 하고 Outline을 비활성화.
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].LockOBJ.SetActive(true);
                parts[i].GetComponent<Outline>().enabled = false;
            }
        
            // 선택한 영웅만 Lock을 비활성화하고 OutLine을 활성화한다.
            s_Part.LockOBJ.SetActive(false);
            s_Part.GetComponent<Outline>().enabled = true;
        }
    }

    public override bool Init()
    {
        InitButtons();
        
        Render_Manager.instance.HERO.InitHero();

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
            parts.Add(go);
            go.Initalize(data.Value, this);
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
