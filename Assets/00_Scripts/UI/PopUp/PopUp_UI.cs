using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 아이템 설명 정보 팝업
public class PopUp_UI : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] private Image IconImage;
    [SerializeField] private TextMeshProUGUI TitleText, RarityText, ExplaneText;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Item_PopUp(Item_Scriptable item, Vector2 pos)
    {
        rect.anchoredPosition = pos;
        IconImage.sprite = Utils.Get_Atlas(item.name);
        TitleText.text = item.name;
        RarityText.text = Utils.String_Color_Rarity(item.rarity) + " 등급</color>";
        ExplaneText.text = item.Item_Des;
    }

    public Vector2 PivotPoint(Vector2 pos)
    {

    }
}
