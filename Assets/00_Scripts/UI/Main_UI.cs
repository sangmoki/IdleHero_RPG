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
        Stage_Manager.m_BossEvent += OnBoss;
        Stage_Manager.m_ClearEvent += OnClear;
        Stage_Manager.m_DeadEvent += OnDead;
    }

    [Header("##Default")]
    // 메인 UI 텍스트 변수
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_AvgDPS_Text;

    [Space(20f)]
    [Header("##Fade")]
    // 페이드 인아웃 작업 위한 변수
    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration;

    [Space(20f)]
    [Header("##Monster_Slider")]
    // 몬스터 처치 수 계산 위한 변수
    [SerializeField] private GameObject m_Monster_Count_Slider_OBJ;
    [SerializeField] private Image m_Monster_Count_Image;
    [SerializeField] private TextMeshProUGUI m_Monster_Count_Text;

    [Space(20f)]
    [Header("##Boss_Slider")]
    // 보스 슬라이더
    [SerializeField] private GameObject m_Boss_Slider_OBJ;
    [SerializeField] private Image m_Boss_HP_Slider;
    [SerializeField] private TextMeshProUGUI m_Boss_HP_Text, m_Boss_Stage_Text;

    [Space(20f)]
    [Header("##Dead_Frame")]
    [SerializeField] private GameObject m_Dead_Frame_OBJ;

    public void Set_Boss_State()
    {
        Stage_Manager.isDead = false;
        Stage_Manager.State_Change(Stage_State.Boss);
    }

    // 보스 클리어 체크
    private void SliderOBJCheck(bool Boss)
    {
        if (Stage_Manager.isDead)
        {
            m_Monster_Count_Slider_OBJ.SetActive(false);
            m_Boss_Slider_OBJ.SetActive(false);

            m_Dead_Frame_OBJ.SetActive(true);
            return;
        }
        m_Dead_Frame_OBJ.SetActive(false);

        m_Monster_Count_Slider_OBJ.SetActive(!Boss);
        m_Boss_Slider_OBJ.SetActive(Boss);

        Monster_Count_Slider();

        float value = Boss ? 1.0f : 0.0f;
        Boss_Slider_Count(value, 1.0f);
    }

    // 보스 생성 시 이벤트
    private void OnBoss()
    {
        SliderOBJCheck(true);
    }

    // 보스 클리어
    private void OnClear()
    {
        SliderOBJCheck(false);
        StartCoroutine(Clear_Delay());
    }
    
    private void OnDead()
    {
        SliderOBJCheck(false);

        StartCoroutine(Dead_Delay());
    }

    IEnumerator Dead_Delay()
    {
        yield return StartCoroutine(Clear_Delay());
        SliderOBJCheck(false);

        for (int i = 0; i < Spawner.m_Monsters.Count; i++)
        {
            // 보스라면 보스 삭제
            if (Spawner.m_Monsters[i].isBoss == true)
            {
                Destroy(Spawner.m_Monsters[i].gameObject);
            }
            // 아니라면 풀링으로 반환
            else
            {
                Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(Spawner.m_Monsters[i].gameObject);
            }
        }
        Spawner.m_Monsters.Clear();
    }

    // 보스 클리어 후 이벤트
    IEnumerator Clear_Delay()
    {
        // 2초뒤 FadeIn
        yield return new WaitForSeconds(2.0f);
        FadeInOut(false);

        // 1초 뒤 Ready State로 전환
        yield return new WaitForSeconds(1.0f);
        Stage_Manager.State_Change(Stage_State.Ready);
    }

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

    // 보스 슬라이더 체력 표시
    public void Boss_Slider_Count(float hp, double maxHp)
    {
        float value = hp / (float)maxHp;

        if (value <= 0.0f)
        {
            value = 0.0f;
        }
        m_Boss_HP_Slider.fillAmount = value;
        m_Boss_HP_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
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
        m_Level_Text.text = "LV." + (Base_Manager.Player.Level + 1).ToString();
        m_AvgDPS_Text.text = StringMethod.ToCurrencyString(Base_Manager.Player.Average_DPS());
    }

}
