using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main_UI : MonoBehaviour
{
    public static Main_UI instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        TextCheck();
        Monster_Count_Slider();

        Stage_Manager.m_ReadyEvent += () => FadeInOut(true);
    }

    // 메인 UI 텍스트 변수
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_AvgDPS_Text;

    // 페이드 인아웃 작업 위한 변수
    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration;

    // 몬스터 처치 수 계산 위한 변수
    [SerializeField] private Image m_Monster_Count_Image;
    [SerializeField] private TextMeshProUGUI m_Monster_Count_Text;

    // 몬스터 처치 슬라이더
    public void Monster_Count_Slider()
    {
        float value = (float)Stage_Manager.Count / (float)Stage_Manager.MaxCount;

        // 몬스터 처치가 100% 이상일 경우 보스 스테이지로 전환
        if (value >= 1.0f)
        {
            value = 1.0f;

            if (Stage_Manager.m_State != Stage_State.Boss)
                Stage_Manager.State_Change(Stage_State.Boss);
        }

        m_Monster_Count_Image.fillAmount = value;
        m_Monster_Count_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }

    // FadeInOut 기능 -> Fade 기능이 어느 위치에서 동작하는지
    public void FadeInOut(bool FadeInOut, bool Sibling = false, Action action = null)
    {
        // Sibling 작업을 통하여 Fade Object의 인덱스 위치를 이동시킨다.
        if (!Sibling)
        {
            m_Fade.transform.parent = this.transform;
            m_Fade.transform.SetSiblingIndex(0);
        } 
        else
        {
            m_Fade.transform.parent = Base_Canvas.instance.transform;
            m_Fade.transform.SetAsLastSibling();
        }

        StartCoroutine(FadeInOut_Coroutine(FadeInOut, action));
    }

    // Color 알파값을 조절하여 FadeInOut 기능을 구현
    IEnumerator FadeInOut_Coroutine(bool FadeInOut, Action action = null)
    {
        // FadeIn일 때 Button 클릭 방지
        if (FadeInOut == false)
        {
            m_Fade.raycastTarget = true;
        }

        float current = 0.0f;
        float percent = 0.0f;
        float start = FadeInOut ? 1.0f : 0.0f; // 1이면 255, 0이면 0
        float end = FadeInOut ? 0.0f : 1.0f; // 1이면 0, 0이면 255
        while (percent < 1.0f)
        {
            // fade 시간 조절
            current += Time.deltaTime;
            percent = current / m_FadeDuration;

            // color 알파값 변경
            float LerpPos = Mathf.Lerp(start, end, percent);
            m_Fade.color = new Color(0, 0, 0, LerpPos);

            yield return null;
        }

        if (action != null) action?.Invoke();

        // FadeInOut일 경우 RaycastTarget을 false로 변경하여 클릭을 막는다.
        m_Fade.raycastTarget = false;
    }

    // 레벨업이 될 때마다 UI 상단 텍스트를 변경
    public void TextCheck()
    {
        m_Level_Text.text = "LV." + (Base_Mng.Player.Level + 1).ToString();
        m_AvgDPS_Text.text = StringMethod.ToCurrencyString(Base_Mng.Player.Average_DPS());
    }

}
