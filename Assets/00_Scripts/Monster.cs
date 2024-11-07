using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float m_Speed;   // 몬스터의 이동속도
    Animator animator;

    bool isSpawn = false;   // 스폰 확인 플래그

    private void Start()
    {
        // animator 객체에 컴포넌트 할당
        animator = GetComponent<Animator>();
        // 몬스터가 스폰될 때 크기가 점점 커지는 효과를 주기 위한 코루틴 함수 실행
        StartCoroutine(Spawn_Start());
    }

    // 몬스터가 스폰될 때 발생하는 코루틴 함수
    // 몬스터가 스폰될 때 크기가 점점 커지는 효과 - 스폰을 좀 더 자연스럽게 하기 위함
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x;

        // percent가 1보다 낮을 때만 실행
        while (percent < 1)
        {
            current += Time.deltaTime;  // 1초에 걸리는 시간
            percent = current / 0.2f;   // 1초에 걸리는 시간을 1로 나누어 0~1 사이의 값으로 변환

            // 시작값(0.0f)에서 끝점(몬스터.x) 까지의 걸리는 특정 시간 속도로 이동
            float LerpPos = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }

    private void Update()
    {
        // 몬스터가 걸어가는 방향을 쳐다본다.
        transform.LookAt(Vector3.zero);

        // isSpawn이 false이면 return
        if (isSpawn == false) return;

        // Distance는 첫번째 인자와 두번째 인자의 사잇값
        // transform.position : 몬스터의 현재 위치
        // Vector3.zero : (0, 0, 0) - 가운데 위치
        // 즉, targetDistance = transform.position과 (0, 0, 0) 사이의 거리
        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);
        if (targetDistance <= 0.5f)
        {
            // 몬스터가 가운데에 도착하면 IDLE 상태로 변경
            AnimatorChange("isIDLE");
        }
        else
        {
            // 몬스터가 (0, 0, 0) - 가운데로 이동
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * m_Speed);
            // 몬스터가 가운데로 이동중이면 MOVE 상태로 변경
            AnimatorChange("isMOVE");
        }
    }

    // 몬스터의 동작 변경 함수
    private void AnimatorChange(string temp)
    {
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }
}
