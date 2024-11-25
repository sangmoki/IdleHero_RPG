using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Character_Scriptable CH_Data; // 캐릭터 데이터
    public GameObject TrailObject;        // 플레이어의 공격을 표시할 Trail 오브젝트    
    public string CH_Name;                // 캐릭터 이름
    Vector3 startPos;                     // 플레이어의 시작 위치
    Quaternion rot;                       // 플레이어의 회전값

    protected override void Start()
    {
        base.Start();

        // 캐릭터의 이름을 통해 Scriptable에 세팅한 캐릭터의 데이터 세팅 
        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/" + CH_Name));

        // 플레이어가 생성되었으니 Spawner에도 전달해주어야 한다.
        // 그래야 몬스터가 추적할 수 있는 대상이 될 수 있다.
        Spawner.m_Players.Add(this);

        // 플레이어의 초기 시작 위치와 회전값을 저장
        // 몬스터를 추적하다가 몬스터가 범위 내에서 사라지면
        // 다시 제자리로 돌아오기 위한 변수
        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Data_Set(Character_Scriptable data)
    {
        CH_Data = data;
        Attack_Range = data.m_Attack_Range;

        Set_ATKHP();
    }

    public void Set_ATKHP()
    {
        ATK = Base_Mng.Player.Get_ATK(CH_Data.m_Rarity);
        HP = Base_Mng.Player.Get_HP(CH_Data.m_Rarity);
    }

    private void Update()
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

        if (Stage_Manager.m_State != Stage_State.Play) return;

        // 가장 가까운 몬스터를 찾아서 타겟으로 지정
        FindClosetTarget(Spawner.m_Monsters.ToArray());

        if (m_Target == null)
        {

        }

        // 만약 타겟의 상태가 사망 상태라면 다시 타겟을 찾는다.
        if (m_Target.GetComponent<Character>().isDead) 
            FindClosetTarget(Spawner.m_Monsters.ToArray());

        // 현재 타겟의 위치와 플레이어의 위치를 계산한 값
        float targetDistance = Vector3.Distance(transform.position, m_Target.position);
        // 현재 타겟이 추적 범위 안에 있지만 공격범위 안에 존재하지 않을 경우
        if (targetDistance <= target_Range && targetDistance > Attack_Range && isATTACK == false)
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

    // 공격 받은 후 이벤트 함수
    public override void GetDamage(double dmg)
    {
        base.GetDamage(dmg);

        var goObj = Base_Mng.Pool.Pooling_Obj("HIT_TEXT").Get((value) =>
        {
            value.transform.position = transform.position;
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, true);
        });

        HP -= dmg;
    }

    protected override void Attack()
    {
        base.Attack();
        TrailObject.SetActive(true);

        Invoke("TrailDisable", 1.0f);   
    }

    private void TrailDisable() => TrailObject.SetActive(false);
}
