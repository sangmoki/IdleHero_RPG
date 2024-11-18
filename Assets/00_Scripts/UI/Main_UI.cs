using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_UI : MonoBehaviour
{
    public static Main_UI instance = null; 

    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_AvgDPS_Text;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TextCheck();
    }

    // 레벨업이 될 때마다 UI 상단 텍스트를 변경
    public void TextCheck()
    {
        m_Level_Text.text = "LV." + (Base_Mng.Player.Level + 1).ToString();
        m_AvgDPS_Text.text = StringMethod.ToCurrencyString(Base_Mng.Player.Average_DPS());
    }
}
