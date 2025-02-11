using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ADS_Buff : UI_Base
{
    public enum ADS_Buff_State
    {
        ATK,
        GOLD,
        CRITICAL
    }

    [SerializeField] private TextMeshProUGUI m_Level_Text, m_Count_Text;
    [SerializeField] private Button[] m_Buttons;
    [SerializeField] private Image[] m_Buttons_Fill;
    [SerializeField] private Image m_Level_Fill;
    [SerializeField] private GameObject[] m_Lock_Button, m_Lock_Frame, m_Lock_Timer;
    [SerializeField] private TextMeshProUGUI[] m_Text_Timer;


    public override bool Init()
    {
        for (int i = 0; i < Data_Manager.m_Data.Buff_timers.Length; i++)
        {
            int index = i;
            m_Buttons[i].onClick.AddListener(() => GetBuff((ADS_Buff_State)index));
            
            if (Data_Manager.m_Data.Buff_timers[i] > 0.0f)
            {

                SetBuff(i, true);
            }
        }

        return base.Init();
    }

    private void Update()
    {
        for (int i = 0; i < Data_Manager.m_Data.Buff_timers.Length; i++)
        {
            // 배속 적용중일 시 
            if (Data_Manager.m_Data.Buff_timers[i] > 0.0f)
            {
                // 1 - (남은 시간 / 총 시간)으로 fillAmount를 계산
                m_Buttons_Fill[i].fillAmount = 1 - (Data_Manager.m_Data.Buff_timers[i] / 1800.0f);

                TimeSpan timespan = TimeSpan.FromSeconds(Data_Manager.m_Data.Buff_timers[i]);
                string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
                m_Text_Timer[i].text = timer;
            }
        }
    }

    // 버프 활성화 시 이벤트
    public void GetBuff(ADS_Buff_State m_State)
    {
        int stateValue = (int)m_State;

        // 버프 횟수 증가
        Data_Manager.m_Data.Buff_Count++;
        // 버프 지속시간 기본 30분
        Data_Manager.m_Data.Buff_timers[(int)m_State] = 1800.0f;
        // 현재 버프 적용 상태 확인
        Main_UI.instance.BuffCheck();
        // 버프 활성화
        SetBuff(stateValue, true);
    }

    // 버프 상태에 따른 버튼 활성화
    void SetBuff(int stateValue, bool GetBool)
    {
        m_Lock_Button[stateValue].SetActive(GetBool);
        m_Lock_Frame[stateValue].SetActive(!GetBool);
        m_Lock_Timer[stateValue].SetActive(GetBool);
    }
}
