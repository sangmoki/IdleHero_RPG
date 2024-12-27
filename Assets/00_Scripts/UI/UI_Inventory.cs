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
        StartCoroutine(BarMovementCoroutine(m_Top_buttons[(int)m_State].GetComponent<RectTransform>().anchoredPosition));
    }

    // 바 움직임 코루틴
    IEnumerator BarMovementCoroutine(Vector2 endPos)
    {
        float current = 0;
        float percent = 0;
        Vector2 start = m_Bar.anchoredPosition;
        Vector2 end = endPos;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.1f;
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
            m_Bar.anchoredPosition = LerpPos;

            yield return null;
        }
    }


}
