using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SavingMode : UI_Base
{
    [SerializeField] private TextMeshProUGUI BatteryText, TimerText, StageText, FightText;
    [SerializeField] private Image BatteryFill;
    [SerializeField] private Transform Content;
    [SerializeField] private UI_Inventory_Part item_part;

    public Dictionary<string, Item> m_SaveItem = new Dictionary<string, Item>();

    // Color(0.0f, 0.0f, 0.0f, 1.0f) -> 검정색
    Color m_Stage_Color = new Color(0, 0.7295136f, 1.0f, 1.0f);

    private void Update()
    {
        // SystemInfo.batteryLevel : 모바일 기준 배터리 잔량 (0.0f ~ 1.0f)
        BatteryText.text = (SystemInfo.batteryLevel * 100.0f).ToString() + "%";
        BatteryFill.fillAmount = SystemInfo.batteryLevel;

        TimerText.text = System.DateTime.Now.ToString("tt hh:mm");

        int stageValue = Base_Manager.Data.Stage + 1;
        int stageForward = (stageValue / 1000) + 1;
        int stageBack = stageValue % 1000;

        StageText.text = "매우어려움 " + stageForward.ToString() + "-" + stageBack.ToString();
        FightText.text = Stage_Manager.isDead ? "반복중..." : "진행중...";
        FightText.color = Stage_Manager.isDead ? Color.yellow : m_Stage_Color;
    }

    public void GetItem(Item_Scriptable item)
    {
        // 인벤토리에 아이템이 있는지 확인함
        if (m_SaveItem.ContainsKey(item.name))
        {
            // 있다면 개수 증가
            m_SaveItem[item.name].Count++;
            return;
        } 
        else
        {
            // 없다면 1개로 새로 생성한다.
            Item items = new Item { data = item, Count = 1 };

            // 아이템을 생성하여 Content에 추가
            m_SaveItem.Add(item.name, items);
            var go = Instantiate(item_part, Content);

            // Init으로 아이템 정보를 전달
            go.Init(items);
        }
    }
}
