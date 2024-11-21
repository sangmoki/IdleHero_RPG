using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heroes_Part : MonoBehaviour
{
    // Hero_Panel의 하위 오브젝트들
    [SerializeField] private Image m_Slider, m_CharacterImage, m_RarityImage;
    [SerializeField] private TextMeshProUGUI m_Level, m_Count;

    public void Initalize(Character_Scriptable data)
    {
        // 레어도와 Sprite 이름에 따른 이미지 변경
        m_RarityImage.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
        m_CharacterImage.sprite = Utils.Get_Atlas(data.m_Character_Name);
        m_CharacterImage.SetNativeSize();
        // 이미지 크기 조정
        RectTransform rect = m_CharacterImage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.3f, rect.sizeDelta.y / 2.3f);
    }
}
