using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base_Canvas : MonoBehaviour
{
    // 싱글톤 패턴을 위한 변수
    public static Base_Canvas instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        HERO_BUTTON.onClick.AddListener(() => Get_UI("#Heroes", true));
        INVENTORY_BUTTON.onClick.AddListener(() => Get_UI("#Inventory"));
        SAVINGMODE_BUTTON.onClick.AddListener(() => {
            Get_UI("#SavingMode");
            isSave = true;
        });
    }

    // 코인의 위치 정보를 저장하기 위한 함수
    public Transform COIN;

    // 순서를 조정하기 위한 레이어 변수
    [SerializeField] private Transform LAYER;
    // UI 하단 SystemBar 버튼
    [SerializeField] private Button HERO_BUTTON, INVENTORY_BUTTON, SAVINGMODE_BUTTON;
    public PopUp_UI popup = null;
    public UI_Base m_UI;
    public static bool isSave = false;

    private void Update()
    {
        // ESC키를 누르면 팝업 UI를 닫는다.
        if (Input.GetKeyDown(KeyCode.Escape))
            Utils.ClosePopupUI();
        else
        {
        }
    }

    // 레이어의 순서 조정 - value가 낮을수록 아래쪽에 깔린다.
    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }

    public void Get_UI(string temp, bool Fade = false)
    {

        if (Fade)
        {
            Main_UI.instance.FadeInOut(false, true, () => Get_PopupUI(temp));
            return;
        }
        Get_PopupUI(temp);
    }

    void Get_PopupUI(string temp)
    {
        if (m_UI != null) m_UI = null;

        // 가져온 UI를 UI_Holder에 저장
        var go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), transform);
        m_UI = go;
        Utils.UI_Holder.Push(go);
    }

    // 아이템 정보 팝업 UI 호출
    public PopUp_UI PopUpItem()
    {
        // 기존 팝업이 있으면 기존 팝업을 파괴하고 생성한다.
        if (popup != null) Destroy(popup.gameObject);

        popup = Instantiate(Resources.Load<PopUp_UI>("UI/PopUp_Item"), transform);
        return popup;
    }
}
