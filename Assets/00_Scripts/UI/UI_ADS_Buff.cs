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

    [SerializeField] private TextMeshProUGUI m_LevelText, m_CountText;
    [SerializeField] private Button[] m_Buttons;
    [SerializeField] private GameObject[] m_Lock_Button, m_Lock_Frame, m_Lock_Timer;
    [SerializeField] private TextMeshProUGUI[] m_Text_Timer;

    float[] timers = { 0.0f, 0.0f, 0.0f };

    private void Start()
    {
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            int index = i;
            m_Buttons[i].onClick.AddListener(() => GetBuff((ADS_Buff_State)index));
        }
    }

    // 버프 활성화 시 이벤트
    public void GetBuff(ADS_Buff_State m_State)
    {
        bool GetBool = true;
        int stateValue = (int)m_State;
        
        // 버프 지속시간 기본 30분
        timers[(int)m_State] = 1800.0f;

        m_Lock_Button[stateValue].SetActive(GetBool);
        m_Lock_Frame[stateValue].SetActive(!GetBool);
        m_Lock_Timer[stateValue].SetActive(GetBool);
    }
}
