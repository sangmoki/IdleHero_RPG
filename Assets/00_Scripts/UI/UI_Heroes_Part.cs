using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 영웅 탭 클래스
public class UI_Heroes_Part : MonoBehaviour
{
    // Hero_Panel의 하위 오브젝트들
    [SerializeField] private Image m_Slider, m_CharacterImage, m_RarityImage;
    [SerializeField] private TextMeshProUGUI m_Level, m_Count;
    [SerializeField] private GameObject GetLock;

    public GameObject LockOBJ;

    public Character_Scriptable m_Character;
    UI_Heroes parent;

    public void Initalize(Character_Scriptable data, UI_Heroes parentBase)
    {
        parent = parentBase;
        m_Character = data;

        // 레어도와 Sprite 이름에 따른 이미지 변경
        m_RarityImage.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
        m_CharacterImage.sprite = Utils.Get_Atlas(data.m_Character_Name);
        m_CharacterImage.SetNativeSize();
        // 이미지 크기 조정
        RectTransform rect = m_CharacterImage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.3f, rect.sizeDelta.y / 2.3f);

        GetCharacterCheck();
    }

    // 착용중인 영웅인지 확인
    public void GetCharacterCheck()
    {
        bool get = false;

        for (int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            if (Base_Manager.Character.m_Set_Character[i] != null)
            {
                if (Base_Manager.Character.m_Set_Character[i].data == m_Character)
                {
                    get = true;
                }
            }
        }
        GetLock.SetActive(get);
    }

    // 영웅 클릭 이벤트
    public void Click_Hero()
    {
        // 영웅을 클릭한다면 PLUS 파티클 실행
        Render_Manager.instance.HERO.GetParticle(true);
        
        parent.OnClickHero(this);
    }
}
