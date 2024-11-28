using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

// IPointerHandler는 마우스 이벤트
public class LevelUp_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    // 경험치 슬라이더
    [SerializeField] private Image m_EXP_Slider;   
    // 각 object 별 텍스트
    [SerializeField] private TextMeshProUGUI EXP_Text, ATK_Text, GoldText, HP_Text, Get_EXP_Text;
    // 마우스 눌림 여부
    bool isPush = false;
    // 눌리는 시간
    float timer = 0.0f;
    // 연속 터치를 진행할 코루틴
    Coroutine coroutine;

    void Start()
    {
        InitEXP();
    }

    void Update()
    {
        // 마우스가 눌렸을 때
        if (isPush)
        {
            // 0.01초마다 연속 터치를 확인
            timer += Time.deltaTime;
            if (timer >= 0.01f)
            {
                timer = 0.0f;
                EXP_UP();
            }
        }
    }

    // 경험치 오르는 함수
    public void EXP_UP()
    {
        Base_Manager.Player.EXP_UP();
        InitEXP();
        // DOTween은 특정 변수 값들을 일정시간동안 원하는 값으로 부드럽게 변화시켜주는 역할
        // 즉, 모션을 부드럽게 만들어준다.
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.25f);
    }

    // 마우스를 눌렀을 때
    public void OnPointerDown(PointerEventData eventData)
    {
        // 경험치 상승 함수
        EXP_UP();
        coroutine = StartCoroutine(Push_Coroutine());
    }

    // 마우스를 뗐을 때
    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    private void InitEXP()
    {
        m_EXP_Slider.fillAmount = Base_Manager.Player.EXP_Percentage();
        EXP_Text.text = string.Format("{0:0.00}%", Base_Manager.Player.EXP_Percentage() * 100.0f);
        ATK_Text.text = "+" + StringMethod.ToCurrencyString(Base_Manager.Player.Next_ATK());
        HP_Text.text = "+" + StringMethod.ToCurrencyString(Base_Manager.Player.Next_HP());
        Get_EXP_Text.text = "<color=#00FF00>EXP</color> +" + string.Format("{0:0.00}", Base_Manager.Player.Next_EXP()) + "%";
    }

    // 마우스 눌림 여부를 확인하는 코루틴
    IEnumerator Push_Coroutine()
    {
        // 1초간 대기하는 이유는 마우스가 계속 눌림 상태인지 뗸 상태인지 구분하기 위함
        yield return new WaitForSeconds(0.5f);
        isPush = true;
    }
}
