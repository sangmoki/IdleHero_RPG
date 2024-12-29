using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    public enum InventoryState
    {
        ALL,
        EQUIPMENT,
        CONSUMABLE,
        OTHER
    }
    [SerializeField]
    public InventoryState m_InventoryState;
    [SerializeField]
    RectTransform m_Bar;
    [SerializeField]
    private Button[] m_Top_buttons;
    [SerializeField]
    RectTransform TopContent;

    [SerializeField]
    Transform Content;
    public UI_Inventory_Part part;

    public override bool Init()
    {
        foreach(var item in Base_Manager.Inventory.m_Items)
        {
            Instantiate(part, Content).Init(item.Value);
        }

        for (int i = 0; i < m_Top_buttons.Length; i++)
        {
            int index = i;
            m_Top_buttons[i].onClick.AddListener(() => Item_Inventory_Check((InventoryState)index));
        }

        return base.Init();
    }

    public void Item_Inventory_Check(InventoryState m_State)
    {
        m_InventoryState = m_State;

        // 바에 Contents Size Filter 컴포넌트를 부여하여 텍스트의 길이만큼 자동으로 조절하게 설정
        StartCoroutine(BarMovementCoroutine(
            m_Top_buttons[(int)m_State].GetComponent<RectTransform>().anchoredPosition,
            m_Top_buttons[(int)m_State].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x));
    }

    // 바 움직임 코루틴
    IEnumerator BarMovementCoroutine(Vector2 endPos, float endXPos)
    {
        float current = 0;
        float percent = 0;
        Vector2 start = m_Bar.anchoredPosition;
        Vector2 end = new Vector2(endPos.x, TopContent.anchoredPosition.y);

        float startX = m_Bar.sizeDelta.x;
        // 바 길이를 글자수 + 50.0f 만큼 설정
        float endX = endXPos + 50.0f;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.1f;
            
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
            float LerpPosX = Mathf.Lerp(startX, endX, percent);

            m_Bar.anchoredPosition = LerpPos;

            // Mathf.Clamp 함수를 사용하여 바의 길이가 0보다 작아지지 않도록 설정
            // Mathf.Clamp(현재값, 최소값, 최대값)
            // 현재 값이 최소값보다 낮거나 최대값보다 높을 수 없다.
            m_Bar.sizeDelta = new Vector2(Mathf.Clamp(LerpPosX, 200.0f, Mathf.Infinity), m_Bar.sizeDelta.y);

            yield return null;
        }
    }


}
