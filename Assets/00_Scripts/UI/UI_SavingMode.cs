using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SavingMode : UI_Base
{
    [SerializeField] private TextMeshProUGUI BatteryText, TimerText, StageText, FightText;
    [SerializeField] private Image BatteryFill, LandImage;
    [SerializeField] private Transform Content;
    [SerializeField] private UI_Inventory_Part item_part;

    public Dictionary<string, Item> m_SaveItem = new Dictionary<string, Item>();
    public Dictionary<string, UI_Inventory_Part> m_Parts = new Dictionary<string, UI_Inventory_Part>();

    // 절전모드 해제 용도
    Vector2 StartPos, EndPos;

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

        StageText.text = "보통 " + stageForward.ToString() + "-" + stageBack.ToString();
        FightText.text = Stage_Manager.isDead ? "반복중..." : "진행중...";
        FightText.color = Stage_Manager.isDead ? Color.yellow : m_Stage_Color;
        
        // 마우스가 한번 눌렸을 때
        if (Input.GetMouseButtonDown(0))
        {
            StartPos = Input.mousePosition;
        }
        // 마우스를 누르는 동안
        if (Input.GetMouseButton(0))
        {
            // 마우스를 누르고 있는 동안 위치
            EndPos = Input.mousePosition;
            float distance = Vector2.Distance(EndPos, StartPos);

            // 거리가 1500.0f 이상이면 비활성화
            LandImage.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(distance / (Screen.width / 2), 0.3f, 0.8f));
            
            if (distance >= Screen.width/2)
            {
                DisableOBJ();
            }
        }
        // 마우스를 눌렀다가 뗀 순간
        if (Input.GetMouseButtonUp(0))
        {
            StartPos = Vector2.zero;
            EndPos = Vector2.zero;
            LandImage.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        }

    }

    public void GetItem(Item_Scriptable item)
    {
        // 인벤토리에 아이템이 있는지 확인함
        if (m_SaveItem.ContainsKey(item.name))
        {
            // 있다면 개수 증가
            m_SaveItem[item.name].Count++;
            m_Parts[item.name].Init(m_SaveItem[item.name]);
            return;
        } 
        // 없다면 1개로 새로 생성한다.
        Item items = new Item { data = item, Count = 1 };

        // 아이템을 생성하여 Content에 추가
        m_SaveItem.Add(item.name, items);
        var go = Instantiate(item_part, Content);
        m_Parts.Add(item.name, go);

        // Init으로 아이템 정보를 전달
        go.Init(items);
    }
}
