using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    bool isSpawn = false;   // 스폰 확인 플래그

    // Unity Engine 내부에서 몬스터 스탯 설정 변수
    public float m_Speed;   // 몬스터의 이동속도
    public double R_ATK, R_HP;
    public float R_Attack_Range;

    public bool isBoss;     // 보스 몬스터인지 확인하는 플래그

    // 초기값 설정
    protected override void Start()
    {
        base.Start();
    }

    // 재활용 - Start에서 실행하면 한번 오브젝트가 나갔다 들어오면 실행이 안됨.
    public void Init()
    {
        // 풀링에서 몬스터가 초기화 될 때 몬스터의 상태 초기화
        isDead = false;
        ATK = R_ATK;
        HP = R_HP;
        Attack_Range = R_Attack_Range;
        target_Range = Mathf.Infinity; 

        // 몬스터가 스폰될 때 크기가 점점 커지는 효과를 주기 위한 코루틴 함수 실행
        StartCoroutine(Spawn_Start());
    }

    private void Update()
    {
        // isSpawn이 false이면 return
        if (isSpawn == false) return;
        // 기존 Play State이거나 Boss_Play일 시 공격 연출 멈춤
        if (Stage_Manager.m_State == Stage_State.Play || Stage_Manager.m_State == Stage_State.Boss_Play)
        {
            // 가장 가까운 플레이어를 찾아서 타겟으로 지정
            if (m_Target == null)
                FindClosetTarget(Spawner.m_Players.ToArray());

            if (m_Target != null)
            {
                // 만약 타겟의 상태가 사망 상태라면 다시 타겟을 찾는다.
                if (m_Target.GetComponent<Character>().isDead)
                {
                    FindClosetTarget(Spawner.m_Players.ToArray());
                }

                // 현재 타겟의 위치와 플레이어의 위치를 계산한 값
                float targetDistance = Vector3.Distance(transform.position, m_Target.position);
                // 현재 타겟이 추적 범위 안에 있지만 공격 범위 안에 존재하지 않을 경우
                if (targetDistance > Attack_Range && isATTACK == false)
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

        /* 가운데를 바라보는 상황 일 때
        // 몬스터가 걸어가는 방향을 쳐다본다.
        // transform.LookAt(Vector3.zero);


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
         */
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

    // 몬스터의 피격 함수
    public override void GetDamage(double dmg)
    {
        // 몬스터가 이미 죽어있으면 return
        if (isDead) return;

        bool critical = Critical(ref dmg);

        // 몬스터가 피격당했을 경우 HitText를 풀링으로 가져와서 피격당한 텍스트를 생성
        Base_Mng.Pool.Pooling_Obj("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, false, critical);
        });

        HP -= dmg;
        
        // 몬스터의 체력이 0이하가 되면 몬스터의 죽음
        if (HP <= 0)
        {
            isDead = true;
            Dead_Event();
        }
    }

    // 몬스터 처치 이벤트
    private void Dead_Event()
    {
        // 보스가 아니라면 카운트 실행
        if (!isBoss)
        {
            // 처치한 몬스터의 수
            Stage_Manager.Count++;

            // 처치한 몬스터 수에 따른 Slider UI 갱신
            Main_UI.instance.Monster_Count_Slider();
        }
        else
        {
            // 보스를 다 잡은 후 스테이지 클리어
            Stage_Manager.State_Change(Stage_State.Clear);
        }


        // 몬스터를 찾지 못하게 하기 위해 몬스터 리스트에서 제거
        Spawner.m_Monsters.Remove(this);

        // 스모크 이펙트 부여
        Base_Mng.Pool.Pooling_Obj("Smoke").Get((value) =>
        {
            value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            // 스모크 이펙트가 끝나고 반환
            Base_Mng.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");
        });

        // 코인 이펙트 부여하여 현재 몬스터의 위치 값 반환
        Base_Mng.Pool.Pooling_Obj("COIN_PARENT").Get((value) =>
        {
            value.GetComponent<COIN_PARENT>().Init(transform.position);
        });

        // 아이템 드롭 이펙트
        for (int i = 0; i < 3; i++)
        {
            Base_Mng.Pool.Pooling_Obj("Item_OBJ").Get((value) =>
            {
                value.GetComponent<Item_OBJ>().Init(transform.position);
            });
        }

        // 몬스터를 풀링으로 반환
        Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
    }

    // 크리티컬 확률 함수
    private bool Critical(ref double dmg)
    {
        // 0.0f ~ 100.0f 사이의 랜덤값을 생성
        float RandomValue = Random.Range(0.0f, 100.0f);

        // 랜덤값이 플레이어의 크리티컬 확률보다 낮다면
        if (RandomValue <= Base_Mng.Player.Critical_Percent)
        {
            // 데미지에 크리티컬 데미지를 곱하여 크리티컬 데미지 적용
            dmg *= Base_Mng.Player.Critical_Damage / 100; 
            return true;
        }

        return false;
    }

}
