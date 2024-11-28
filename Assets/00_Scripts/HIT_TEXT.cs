using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HIT_TEXT : MonoBehaviour
{
    // 타겟 대상의 위치
    Vector3 target;
    // 카메라에 비춰지는 몬스터의 위치를 받아오기 위한 변수
    Camera cam;
    public TextMeshProUGUI m_Text;

    [SerializeField] private GameObject m_Critical;

    // Inspector를 꾸미는 용도
    // [Header("테스트!")] - 헤더
    // [Space(20f)]        - 거리
    // [Range(0.0f, 5.0f)] // - 범위
    float UpRange = 0.0f; // 텍스트가 위로 올라가는 속도
    // 일정 부분의 값을 더해준다. (위로 방향)

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(Vector3 pos, double dmg, bool monster = false, bool Critical = false)
    {
        // 텍스트 텍스트 위치 랜덤 설정 - 가시성 용이함 위해
        pos.x += Random.Range(-0.1f, 0.1f);
        pos.z += Random.Range(-0.1f, 0.1f);

        target = pos;  // 타겟의 위치 정보
        m_Text.text = StringMethod.ToCurrencyString(dmg); // DMG 텍스트 표시

        // 몬스터와 플레이어의 공격 텍스트 구분
        if (monster) m_Text.color = Color.red;
        else m_Text.color = Color.white;

        transform.parent = Base_Canvas.instance.HOLDER_LAYER(1); // 캔버스의 자식으로 설정

        // 크리티컬 bool 결과에 따른
        m_Critical.SetActive(Critical);
        
        // 피격 텍스트 색상 설정
        Color color_yellow = Color.yellow;
        Color color_white = Color.white;

        // 크리티컬일 경우 노란색으로 변경
        if (Critical) 
            m_Text.colorGradient = new VertexGradient(color_yellow, color_yellow, color_white, color_white);
        else
            m_Text.colorGradient = new VertexGradient(color_white, color_white, color_white, color_white);

        // 사용이 끝나면 2초 후에 반환
        Base_Manager.instance.Return_Pool(2.0f, this.gameObject, "HIT_TEXT"); 
    }

    private void Update()
    {
        // 텍스트가 몬스터보다 위로 보이게 설정
        Vector3 tragetPost = new Vector3(target.x, target.y + UpRange, target.z);
        // 월드좌표로 화면에 표시
        transform.position = cam.WorldToScreenPoint(tragetPost); 
        if (UpRange <= 0.3f)
        {
            // 피격 텍스트가 위로 올라가는 속도
            UpRange += Time.deltaTime;
        }
    }

    // 오브젝트 풀에 반환
    private void ReturnText()
    {
        Base_Manager.Pool.m_pool_Dictionary["HIT_TEXT"].Return(this.gameObject);
    }
}
