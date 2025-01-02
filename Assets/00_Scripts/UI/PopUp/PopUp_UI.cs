using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private void Update()
    {
        // 팝업을 닫는다.
        if (Input.GetMouseButtonDown(0))
        {
            Base_Canvas.instance.popup = null;
            Destroy(this.gameObject);
        }
    }

    public void Item_PopUp(Item_Scriptable item, Vector2 pos)
    {
        rect.pivot = PivotPoint(pos);

        rect.anchoredPosition = pos;
        IconImage.sprite = Utils.Get_Atlas(item.name);
        TitleText.text = item.name;
        RarityText.text = Utils.String_Color_Rarity(item.rarity) + item.rarity.ToString() + "</color>";
        ExplaneText.text = item.Item_Des;
    }

    // 팝업 위치 조정 (화면 중앙 기준으로)
    public Vector2 PivotPoint(Vector2 pos)
    {
        float xPos = pos.x > Screen.width / 2 ? 1.0f : 0.0f;
        float yPos = pos.y > Screen.height / 2 ? 1.0f : 0.0f;

        return new Vector2(xPos, yPos);
    }
}
