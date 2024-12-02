using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        // UI 텍스트 초기화
        TextCheck();
        // 몬스터 처치 슬라이더 초기화
        Monster_Count_Slider();

        // 아이템 콘텐트의 자식들을 가져와서 리스트에 저장
        for (int i = 0; i < m_Item_Content.childCount; i++)
            m_Item_Texts.Add(m_Item_Content.GetChild(i).GetComponent<TextMeshProUGUI>());

        // 이벤트 연결
        Stage_Manager.m_ReadyEvent += () => FadeInOut(true);
        Stage_Manager.m_BossEvent += OnBoss;
        Stage_Manager.m_ClearEvent += OnClear;
        Stage_Manager.m_DeadEvent += OnDead;
    }

    [Header("##Default")]
    // 메인 UI 텍스트 변수
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_AvgDPS_Text;
    [SerializeField] private TextMeshProUGUI m_LevelUp_Money_Text;
    [SerializeField] private TextMeshProUGUI m_Gold_Text;

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
    // 죽었을 때 나오는 프레임
    [SerializeField] private GameObject m_Dead_Frame_OBJ;

    [Space(20f)]
    [Header("##Legendary_PopUp")]
    // 레전더리 등급 아이템 획득 시 애니메이션
    [SerializeField] private Animator m_Legendary_PopUp;
    [SerializeField] private Image m_Item_Frame;
    [SerializeField] private Image m_PopUp_Image;
    [SerializeField] private TextMeshProUGUI m_PopUp_Text;
    Coroutine Legendary_Coroutine;

    bool isPopUp = false;

    [Space(20f)]
    [Header("##Item_PopUp")]
    [SerializeField] private Transform m_Item_Content;
    private List<TextMeshProUGUI> m_Item_Texts = new List<TextMeshProUGUI>();

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
        m_Level_Text.text = "LV." + (Base_Manager.Data.Level + 1).ToString();
        m_AvgDPS_Text.text = StringMethod.ToCurrencyString(Base_Manager.Player.Average_DPS());

        // 현재 가지고 있는 코인에 따라 색상 변경
        double levelupMoneyValue = Utils.Data.levelData.MONEY();
        m_LevelUp_Money_Text.text = StringMethod.ToCurrencyString(levelupMoneyValue);
        m_LevelUp_Money_Text.color = Utils.CoinUpgradeCheck(levelupMoneyValue) ? Color.white : Color.red;
        
        m_Gold_Text.text = StringMethod.ToCurrencyString(Base_Manager.Data.Money);
    }

    // 레전더리 아이템 획득 시 팝업 이벤트
    public void GetLegendaryPopUp(Item_Scriptable item)
    {
        if (isPopUp)
        {
            m_Legendary_PopUp.gameObject.SetActive(false);
        }
        isPopUp = true;

        // 팝업 활성화
        m_Legendary_PopUp.gameObject.SetActive(true);

        // 아틀라스에 저장한 이미지 불러오기
        m_Item_Frame.sprite = Utils.Get_Atlas(item.rarity.ToString());
        m_PopUp_Image.sprite = Utils.Get_Atlas(item.name);

        // 이미지 사이즈 조절
        m_PopUp_Image.SetNativeSize();

        // 아이템 이름과 레어도 색상 변경
        m_PopUp_Text.text = Utils.String_Color_Rarity(item.rarity) + item.Item_Name + "</color> 을(를) 획득하였습니다.";

        // 중복된 코루틴이 종료되지 않도록 중지
        if (Legendary_Coroutine != null) 
        { 
            StopCoroutine(Legendary_Coroutine);
        }
        Legendary_Coroutine = StartCoroutine(Legendary_PopUp_Coroutine());
    }

    public void GetItem(Item_Scriptable item)
    {
        for (int i = 0; i < m_Item_Texts.Count; i++)
        {
            if (m_Item_Texts[i].gameObject.activeSelf == false)
            {
                m_Item_Texts[i].gameObject.SetActive(true);
                m_Item_Texts[i].text =  
                    "아이템을 획득하였습니다. " 
                    + Utils.String_Color_Rarity(item.rarity) 
                    + "[" + item.Item_Name + "]</color>";

                // 아이템 획득 시 텍스트 위치 변경 -> 5개의 텍스트를 위로 50만큼씩 이동
                for (int j = 0; j < i; j++)
                {
                    RectTransform rect = m_Item_Texts[j].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 50.0f);
                }
                StartCoroutine(Item_Text_FadeOut(m_Item_Texts[i].GetComponent<RectTransform>()));
                
                break;
            }
        }

        // 아이템의 레어도가 레어 이상일 경우 팝업 이벤트
        if ((int)item.rarity >= (int)Rarity.Rare)
            GetLegendaryPopUp(item);
    }

    // 레전더리 아이템 획득 후 종료
    IEnumerator Legendary_PopUp_Coroutine()
    {
        yield return new WaitForSeconds(2.0f);
        isPopUp = false;
        m_Legendary_PopUp.SetTrigger("Close");
    }

    // 아이템 획득 텍스트 이후 사라짐 이벤트
    IEnumerator Item_Text_FadeOut(RectTransform rect)
    {
        // 2초 뒤 텍스트가 사라지면서 원래 위치로 이동
        yield return new WaitForSeconds(2.0f);
        rect.gameObject.SetActive(false);
        rect.anchoredPosition = new Vector2(0.0f, 0.0f);
    }

}
