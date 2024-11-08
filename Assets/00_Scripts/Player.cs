using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Vector3 startPos;  // 플레이어의 시작 위치
    Quaternion rot;    // 플레이어의 회전값

    protected override void Start()
    {
        base.Start();

        // 플레이어의 초기 시작 위치와 회전값을 저장
        // 몬스터를 추적하다가 몬스터가 범위 내에서 사라지면
        // 다시 제자리로 돌아오기 위한 변수
        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Update()
    {
        // 가장 가까운 몬스터를 찾아서 타겟으로 지정
        FindClosetTarget(Spawner.m_Monsters.ToArray());

        if (m_Target == null)
        {
            // 현재 위치와 시작 위치를 계산한 값
            float targetPos = Vector3.Distance(transform.position, startPos);
            if (targetPos > 0.1f)
            {
                // 제자리로 이동
                transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                transform.LookAt(startPos);
                AnimatorChange("isMOVE");
            } 
            else
            {
                // 다시 돌아왔다면 대기상태로 전환
                transform.rotation = rot;
                AnimatorChange("isIDLE");
            }
            return;
        }

        // 만약 타겟의 상태가 사망 상태라면 다시 타겟을 찾는다.
        if (m_Target.GetComponent<Character>().isDead) 
            FindClosetTarget(Spawner.m_Monsters.ToArray());

        // 현재 타겟의 위치와 플레이어의 위치를 계산한 값
        float targetDistance = Vector3.Distance(transform.position, m_Target.position);
        // 현재 타겟이 추적 범위 안에 있지만 공격범위 안에 존재하지 않을 경우
        if (targetDistance <= Target_Range && targetDistance > Attack_Range && isATTACK == false)
        {
            // 타겟을 향해 이동
            AnimatorChange("isMOVE");
            transform.LookAt(m_Target.position);
            transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
        }
        else if (targetDistance <= Attack_Range && isATTACK == false)
        {
            // 타겟이 공격 범위 안에 존재할 경우
            // 공격
            isATTACK = true;
            AnimatorChange("isATTACK");

            // 공격 후 공격 상태 초기화 함수 호출
            Invoke("InitAttack", 1.0f);
        }
    }
}
