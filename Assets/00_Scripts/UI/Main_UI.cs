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
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TextCheck();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FadeInOut(true);
            Debug.Log("Test");
        } 
        else if (Input.GetKeyDown(KeyCode.S))
        {
            FadeInOut(false);
            Debug.Log("Test22");
        }
    }

    // 메인 UI 텍스트 변수
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_AvgDPS_Text;

    // 페이드 인아웃 작업 위한 변수
    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration;

    public void FadeInOut(bool FadeInOut)
    {
        StartCoroutine(FadeInOut_Coroutine(FadeInOut));
    }

    IEnumerator FadeInOut_Coroutine(bool FadeInOut, Action action = null)
    {
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
    }

    // 레벨업이 될 때마다 UI 상단 텍스트를 변경
    public void TextCheck()
    {
        m_Level_Text.text = "LV." + (Base_Mng.Player.Level + 1).ToString();
        m_AvgDPS_Text.text = StringMethod.ToCurrencyString(Base_Mng.Player.Average_DPS());
    }

}
