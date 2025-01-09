using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COIN_PARENT : MonoBehaviour
{
    Vector3 target;
    Camera cam;                                     
    RectTransform[] childs = new RectTransform[5]; // 코인 연출 위한 변수

    [Range(0.0f, 500.0f)]
    [SerializeField] private float m_Distance_Range, speed; // 코인이 튀어나가는 범위, 속도

    private void Awake()
    {
        cam = Camera.main;

        // 자식 오브젝트의 RectTransform 컴포넌트를 가져온다.
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }

    }

    private void OnSave()
    {
        if (Base_Canvas.isSave)
        {
            Base_Manager.Data.Money += Utils.Data.stageData.MONEY();
            if (Distance_Boolean_World(0.5f))
            {
                Base_Manager.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
            }
        }
    }

    private void OnDisable()
    {
        UI_SavingMode.m_OnSaving -= OnSave;
    }

    public void Init(Vector3 pos)
    {
        UI_SavingMode.m_OnSaving += OnSave;

        if (Base_Canvas.isSave) return;

        target = pos;

        // 캔버스 좌표로 변경
        transform.position = cam.WorldToScreenPoint(target);

        // 코인들 각각의 위치를 초기화해준다.
        for (int i = 0; i < 5; i++)
        {
            childs[i].anchoredPosition = Vector2.zero;
        }

        // 캔버스의 자식으로 설정
        transform.parent = Base_Canvas.instance.HOLDER_LAYER(0);

        Base_Manager.Data.Money += Utils.Data.stageData.MONEY();

        StartCoroutine(Coin_Effect());
    }

    IEnumerator Coin_Effect()
    {
        Vector2[] RandomPos = new Vector2[childs.Length];
        for (int i = 0; i < childs.Length; i++)
        {
            // 타겟의 위치 + 랜덤한 위치의 랜덤한 범위
            RandomPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-m_Distance_Range, m_Distance_Range);
        }

        // 모든 코인들이 도착지점에 도착하는 모션
        while (true)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                // 각각 RectTransform을 가져온다.
                RectTransform rect = childs[i];
                // rect의 anchoredPosition을 RandomPos[i]로 이동시킨다.
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, RandomPos[i], Time.deltaTime * speed);
            }

            // 모든 코인들이 도착지점에 도착했다면 break
            if (Distance_Boolean(RandomPos, 0.5f)) break;

            // 한 번의 프레임을 대기
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // 코인들이 도착지점에 도착 후에 한곳으로 이동하는 모션
        while (true)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                // 기존 anchoredPosition은 부모의 로컬 좌표 기준이기 때문에
                // World 좌표인 position을 사용
                rect.position = Vector2.MoveTowards(rect.position, Base_Canvas.instance.COIN.position, Time.deltaTime * (speed * 15));
            }

            if (Distance_Boolean_World(0.5f))
            {
                Base_Manager.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
                break;
            }
            yield return null;
        }
        Main_UI.instance.TextCheck();
    }

    // 로컬 좌표값 기준 (anchoredPosition)
    private bool Distance_Boolean(Vector2[] end, float range)
    {
        for (int i = 0; i < childs.Length; i++)
        {
            // 각각의 거리 값이 Range보다 크면 false를 반환한다.
            // 즉, 하나라도 도착지점에 도착하지 못했다면 false, 아니라면 true
            float distance = Vector2.Distance(childs[i].anchoredPosition, end[i]);
            if (distance > range)
            {
                return false;
            }
        }
        return true;
    }

    // 월드 좌표값 기준 - 위에는 부모를 바라보고 움직여야 했지만
    // 지금은 코인이 모여서 고정된 코인에 들어가야 하기 때문
    private bool Distance_Boolean_World (float range)
    {
        for (int i = 0; i < childs.Length; i++)
        {
            // 즉, 하나라도 도착지점에 도착하지 못했다면 false, 아니라면 true
            float distance = Vector2.Distance(childs[i].position, Base_Canvas.instance.COIN.position);
            if (distance > range)
            {
                return false;
            }
        }
        return true;
    }
}
