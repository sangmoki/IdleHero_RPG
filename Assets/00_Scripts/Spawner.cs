using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int m_Count;         // 몬스터의 수
    public float m_SpawnTime;   // 몇 초 마다 생성할 것인지

    public static List<Monster> m_Monsters = new List<Monster>();
    public static List<Player> m_Players = new List<Player>();

    Coroutine coroutine;

    private void Start()
    {
        Stage_Manager.m_PlayEvent += OnPlay;
        Stage_Manager.m_BossEvent += OnBoss;
    }

    // 게임 시작 시 몬스터 스폰
    public void OnPlay()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }

    // 몬스터 처치 수가 일정량에 도달하면 보스 스테이지 진입
    public void OnBoss()
    {
        // 현재 코루틴이 실행중이라면 코루틴 중지
        if (coroutine != null)
            StopCoroutine(coroutine);

        // 화면상의 몬스터들을 전부 풀로 되돌린다 (제거)
        for (int i = 0; i < m_Monsters.Count; i++)
        {
            Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(m_Monsters[i].gameObject);
        }
        m_Monsters.Clear();

        StartCoroutine(BossSpawnCoroutine());
    }

    // 보스 소환
    IEnumerator BossSpawnCoroutine()
    {
        yield return new WaitForSeconds(3.0f);

        // 보스 생성
        var boss = Instantiate(Resources.Load<Monster>("Pool_OBJ/Boss"), Vector3.zero, Quaternion.Euler(0, 180, 0));
        boss.Init();

        Vector3 bossPos = boss.transform.position;

        // 플레이어 넉백
        for (int i = 0; i < m_Players.Count;i++)
        {
            if (Vector3.Distance(bossPos, m_Players[i].transform.position) <= 2.0f)
            {
                m_Players[i].Knockback(bossPos);
            }
        }
    }

    // 몬스터 스폰 코루틴 함수
    // 몬스터의 스폰 기준
    // 1. 몬스터는 여러 마리가 몇 초 마다 수시로 스폰되어야 한다.
    IEnumerator SpawnCoroutine()
    {
        Vector3 pos;

        // Random.insideUnitSphere == Vector3(x, y, z) <- Vector3 기준 랜덤좌표
        // Random.insideUnitCircle == Vector2(x, y) <- Vector2 기준 랜덤좌표
        for (int i = 0; i < m_Count; i++)
        {
            // 몬스터는 가운데를 기점으로 원형으로 생성되어야 한다.
            // 5~10의 값 = 생성되는 반원의 위치 값
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f; 
            // pos의 y의 값이 -나 +가 되면 안되기 때문에 0으로 초기화
            pos.y = 0.0f;

            // 몬스터가 생성된 위치가 중앙점과 가까우면 pos좌표를 다시 생성한다.
            // 3~5의 값 = 중앙점을 기준으로 생성되는 위치 값의 반경
            while (Vector3.Distance(pos, Vector3.zero) <= 3.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0.0f;
            }

            // 회전값을 가지지 않은 상태로 pos좌표에 몬스터 생성
            // Instantiate (생성자) Destroy (파괴자)
            // 위 두개는 GC(Garbage Collection)이 발생한다.
            // 계속 누적되다보면 메모리가 과부화 된다.
            // 이것을 방지하기 위해 풀링 기법(Pool_Mng)을 사용한다.
            var goObj = Base_Mng.Pool.Pooling_Obj("Monster").Get((value) =>
            {
                // 가져온 Object 몬스터 생성
                value.GetComponent<Monster>().Init();
                // 몬스터의 위치를 pos로 설정
                value.transform.position = pos;
                // 몬스터가 가운데를 바라보도록 설정
                value.transform.LookAt(Vector3.zero);
                // 몬스터 리스트에 몬스터 추가
                m_Monsters.Add(value.GetComponent<Monster>());
            });

            // 몬스터 풀링 코루틴 실행
            // StartCoroutine(ReturnCoroutine(goObj));
        }

        // 스폰 시간만큼 대기 후 스폰 실행
        yield return new WaitForSeconds(m_SpawnTime);
        coroutine = StartCoroutine(SpawnCoroutine());
    }

    // 몬스터를 풀링하기 위한 코루틴 함수
    //IEnumerator ReturnCoroutine(GameObject obj)
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(obj);
    //}
}
