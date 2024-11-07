using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // 몬스터의 스피드
    public float m_Speed;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        // 몬스터가 걸어가는 방향을 쳐다본다.
        transform.LookAt(Vector3.zero);

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
