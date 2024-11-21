using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;

    // 코인의 위치 정보를 저장하기 위한 함수
    public Transform COIN;

    // 순서를 조정하기 위한 레이어 변수
    [SerializeField] private Transform LAYER;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private void Update()
    {
        // ESC키를 누르면 팝업 UI를 닫는다.
        if (Input.GetKeyDown(KeyCode.Escape))
            Utils.ClosePopupUI();
        else
        {
            Debug.Log("게임 종료 팝업 노출");
        }
    }

    // 레이어의 순서 조정 - value가 낮을수록 아래쪽에 깔린다.
    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }

    public void GetUI(string temp)
    {
        // 가져온 UI를 UI_Holder에 저장
        var go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), transform);
        Utils.UI_Holder.Push(go);
    }
}
