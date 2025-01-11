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

        Fast_Lock.gameObject.SetActive(!Base_Manager.isFast);
        Fast_Fade.SetActive(Base_Manager.isFast);

        // 아이템 콘텐트의 자식들을 가져와서 리스트에 저장
        for (int i = 0; i < m_Item_Content.childCount; i++)
        {
            m_Item_Texts.Add(m_Item_Content.GetChild(i).GetComponent<TextMeshProUGUI>());
            m_Item_Coroutines.Add(null);
        }

        // 이벤트 연결
        Stage_Manager.m_ReadyEvent += OnReady;
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
    [SerializeField] private TextMeshProUGUI m_Stage_Count_Text;
    [SerializeField] private TextMeshProUGUI m_Stage_Text;
    // Color = (float, float, float, float)
    //            R      G      B      A
    // Color(0.0f, 0.0f, 0.0f, 1.0f) -> 검정색
    Color m_Stage_Color = new Color(0, 0.7295136f, 1.0f, 1.0f);

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
    private List<Coroutine> m_Item_Coroutines = new List<Coroutine>();

    [Space(20f)]
    [Header("##Hero_Frame")]
    [SerializeField] private UI_Main_Part[] m_Main_Parts;
    public Image Main_Character_Skill_Fill;
    Dictionary<Player, UI_Main_Part> m_Part = new Dictionary<Player, UI_Main_Part>();

    [Header("## ADS")]
    // 배속
    [SerializeField] private Image Fast_Lock;
    [SerializeField] private GameObject Fast_Fade;


    public void GetFast()
    {
        bool fast = !Base_Manager.isFast;
        Base_Manager.isFast = fast;

        Fast_Lock.gameObject.SetActive(!fast);
        Fast_Fade.SetActive(fast);

        // 게임의 속도 조정
        Time.timeScale = fast ? 1.5f : 1.0f;
    }

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

    // 보스 클리어 후 준비상태
    private void OnReady()
    {
        FadeInOut(true);

        m_Part.Clear();

        for (int i = 0; i < 6; i++) m_Main_Parts[i].Initalize();
        
        int indexValue = 0;

        for (int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Manager.Character.m_Set_Character[i];
            if (data != null)
            {
                indexValue++;
                m_Main_Parts[i].InitData(data.Data, false);
                // 데이터가 있을때에만 + 하여 순서 조정
                m_Main_Parts[i].transform.SetSiblingIndex(indexValue);
                m_Part.Add(Character_Spawner.players[i], m_Main_Parts[i]);
            }
        }
    }

    public void SetCharacterData()
    {
        int indexValue = 0;

        for (int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Manager.Character.m_Set_Character[i];
            if (data != null)
            {
                indexValue++;
                m_Main_Parts[i].InitData(data.Data, true);
                m_Main_Parts[i].transform.SetSiblingIndex(indexValue);
            }
        }
    }

    public void CharaterStateCheck(Player player)
    {
        m_Part[player].StateCheck(player);
    }

    // 보스 생성 시 이벤트
    private void OnBoss()
    {
        TextCheck();
        SliderOBJCheck(true);
    }

    // 보스 클리어
    private void OnClear()
    {
        SliderOBJCheck(false);
        StartCoroutine(Clear_Delay());
    }
    
    // 캐릭터 사망
    private void OnDead()
    {
        TextCheck();
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

        if (value == null)
            value = 0.0f;

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

        m_Stage_Text.text = Stage_Manager.isDead ? "반복중..." : "진행중...";
        m_Stage_Text.color = Stage_Manager.isDead ? Color.yellow : m_Stage_Color;

        // 100 스테이지마다 앞에 1이 변경됨
        // ex ) 200
        // 1-100
        // ex) 201
        // 2-1 스테이지로 되어야 함
        // 그럼 Stage % 100 + 1
        int stageValue = Base_Manager.Data.Stage + 1;
        int stageForward = (stageValue / 1000) + 1;
        int stageBack = stageValue % 1000;

        m_Stage_Count_Text.text = "매우어려움 " + stageForward.ToString() + "-" + stageBack.ToString();
        m_Boss_Stage_Text.text = stageForward.ToString() + "-" + stageBack.ToString() + " Stage";
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
        // 모든 텍스트가 활성화 되어있는지 확인
        bool AllActive = true;

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

                // 한번 실행된 코루틴이 반복되지 않도록 중지
                if (m_Item_Coroutines[i] != null)
                    StopCoroutine(m_Item_Coroutines[i]);

                m_Item_Coroutines[i] = StartCoroutine(Item_Text_FadeOut(m_Item_Texts[i].GetComponent<RectTransform>()));
                AllActive = false;
                break;
            }
        }

        // 모든 텍스트가 활성화되어있다면 다시 위치를 초기화 하여 시작
        if (AllActive)
        {
            // 텍스트 위치 초기화 위한 변수
            GameObject baseRect = null;
            float yCount = 0.0f;

            // 가장 높은 위치에 있는 Text를 찾아서 baseRect에 저장
            for (int i = 0; i < m_Item_Texts.Count; i++)
            {
                RectTransform rect = m_Item_Texts[i].GetComponent<RectTransform>();
                if (rect.anchoredPosition.y > yCount)
                {
                    baseRect = rect.gameObject;
                    yCount = rect.anchoredPosition.y;
                }
            }

            for (int i = 0; i < m_Item_Texts.Count; i++)
            {
                if (baseRect == m_Item_Texts[i].gameObject)
                {
                    m_Item_Texts[i].gameObject.SetActive(false);
                    m_Item_Texts[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

                    m_Item_Texts[i].gameObject.SetActive(true);
                    m_Item_Texts[i].text =
                        "아이템을 획득하였습니다. "
                        + Utils.String_Color_Rarity(item.rarity)
                        + "[" + item.Item_Name + "]</color>";
                    
                    // StartCoroutine(Item_Text_FadeOut(m_Item_Texts[0].GetComponent<RectTransform>()));
                }
                else
                {
                    RectTransform rect = m_Item_Texts[i].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 50.0f);
                }
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
