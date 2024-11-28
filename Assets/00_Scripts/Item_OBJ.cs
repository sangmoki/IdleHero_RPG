using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item_OBJ : MonoBehaviour
{
    [SerializeField] private Transform ItemTextRect;      // 아이템 변수
    [SerializeField] private TextMeshProUGUI m_Text;      // 아이템의 이름
    [SerializeField] private GameObject[] Raritys;        // 아이템의 레어도
    [SerializeField] private ParticleSystem m_Loot;       // 아이템을 획득할 때 나오는 이펙트
    [SerializeField] private float firingAngle = 45.0f;   // 곡선
    [SerializeField] private float gravity = 9.8f;        // ridigd(default 9.8)중력 
    
    Rarity rarity;                                        // 아이템의 레어도
    bool isCheck = false;                                 // 아이템이 이동되었는지 체크하는 플래그


    // 레어도 확인
    void RarityCheck()
    {
        // 아이템 이동 확인
        isCheck = true;

        // 아이템 효과 연출을 위해 회전을 시켜놓았기 때문에
        // 아이템의 방향 초기화(0, 0, 0)
        transform.rotation = Quaternion.identity;

        // 랜덤으로 설정된 레어도에 따라 이미지 활성화
        Raritys[(int)rarity].SetActive(true);

        ItemTextRect.gameObject.SetActive(true);
        ItemTextRect.parent = Base_Canvas.instance.HOLDER_LAYER(2);

        // 텍스트에 color와 텍스트 문구 변경
        // <color=#00FF00>TEST ITEM</color> 과 같다
        m_Text.text = Utils.String_Color_Rarity(rarity) + "TEST ITEM" + "</color>";

        StartCoroutine(LootItem());
    }

    // 아이템 줍기 이펙트 활성화
    IEnumerator LootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        // 루팅이 진행되면 레어리티 관련 이펙트 비활성화
        for (int i = 0; i < Raritys.Length; i++)
            Raritys[i].SetActive(false);

        // 이 오브젝트에 캔버스를 가져온다.
        ItemTextRect.transform.parent = this.transform;
        // 아이템을 비활성화
        ItemTextRect.gameObject.SetActive(false);

        // 아이템이 비활성화되면 루팅 이펙트를 실행
        m_Loot.Play();
        // 0.5초 뒤에 루팅 이펙트를 풀에 반환한다.
        yield return new WaitForSeconds(0.5f);
        Base_Manager.Pool.m_pool_Dictionary["Item_OBJ"].Return(this.gameObject);
    }

    private void Update()
    {
        if (isCheck == false) return;

        // 아이템은 이미지 이기 때문에 캔버스에 렌더링 되어야 한다.
        ItemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Init(Vector3 pos)
    {
        // 랜덤으로 레어도 설정
        rarity = (Rarity)Random.Range(0, 5);

        // 이동 확인 초기화
        isCheck = false;
        // 몬스터가 사망한 위치로 대입
        transform.position = pos;
        // 몬스터의 위치가 아닌 몬스터 위치 주변에 랜덤한 좌표로 아이템이 떨어지기 위한 위치 값
        Vector3 Target_Pos = new Vector3(pos.x + (Random.insideUnitSphere.x * 1.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 1.0f));
        // 어느 위치로 이동을하면서 곡선을 그릴 것이냐
        StartCoroutine(SimulateProjectile(Target_Pos));
    }

    // 곡선을 그리는 함수
    IEnumerator SimulateProjectile(Vector3 pos)
    {
        // Mathf.Abs(절대값), Mathf.Min,Max(최솟값, 최댓값), Mathf.Sign(음수 -1, 양수 +1)

        // Mathf.Sin(사인)
        // Mathf.Cos(코스)
        // Mathf.Deg2Rad (각도(degree) -> 호도(radian)) : Degree to Radian 의 약어
        // Mathf.Rad2Deg(호도(radian)->각도(degree)) : Radian to Degree 의 약어
        // Mathf.Sqrt(제곱근)

        // 아이템이 떨어질 위치 값
        float target_Distance = Vector3.Distance(transform.position, pos);

        // 떨어지는 속도 
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // 제곱긍르 Cos와 Sin으로 각각 곱한 값
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // 타겟의 위치(길이) 값
        float flightDuration = target_Distance / Vx;

        // 기존에는 한곳만을 바라보고 움직여서
        // 이동하는 방향으로 회전
        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        // 길이를 이동하기 위한 시간
        float time = 0.0f;

        // time이 flightDuration보다 높아질 때까지 반복
        while (time < flightDuration)
        {
            // 1초마다 더해준다.
            // 만약 flightDuration의 값이 2.5라면 2.5초에 도달할 때까지 반복하여 x축과 y축을 이동할거다.
            // y는 아래로 떨어지고 x는 이동한다.
            transform.Translate(0, (Vy - (gravity * time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        RarityCheck();
    }
}
