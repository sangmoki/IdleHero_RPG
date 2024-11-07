using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject monster_Prefab;

    public int m_Count;         // 몬스터의 수
    public float m_SpawnTime;   // 몇 초 마다 생성할 것인지

    private void Start()
    {
        // 몬스터 스폰 코루틴 실행
        StartCoroutine(SpawnCoroutine());
    }

    // 몬스터가 스폰될 때 발생하는 코루틴 함수
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;   
        float start = 0.0f;
        float end = transform.localScale.x;

        // percent가 1보다 낮을 때만 실행
        while(percent < 1)
        {
            current += Time.deltaTime;  // 1초에 걸리는 시간
            percent = current / 1.0f;   // 1초에 걸리는 시간을 1로 나누어 0~1 사이의 값으로 변환

            // 시작값(0.0f)에서 끝점(몬스터.x) 까지의 걸리는 특정 시간 속도로 이동
            float LerpPos = Mathf.Lerp(start, end, percent); 
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
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
            pos = Vector3.zero + Random.insideUnitSphere * 10.0f; 
            // pos의 y의 값이 -나 +가 되면 안되기 때문에 0으로 초기화
            pos.y = 0.0f;

            // 몬스터가 생성된 위치가 중앙점과 가까우면 pos좌표를 다시 생성한다.
            // 3~5의 값 = 중앙점을 기준으로 생성되는 위치 값의 반경
            while (Vector3.Distance(pos, Vector3.zero) <= 5.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 10.0f;
                pos.y = 0.0f;
            }

            // 회전값을 가지지 않은 상태로 pos좌표에 몬스터 생성
            var go = Instantiate(monster_Prefab, pos, Quaternion.identity);
        }

        // 스폰 시간만큼 대기 후 스폰 실행
        yield return new WaitForSeconds(m_SpawnTime);
        StartCoroutine(SpawnCoroutine());
    }

}
