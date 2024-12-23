using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Character_Scriptable CH_Data; // 캐릭터 데이터
    public ParticleSystem Provocation_Effect; // 보스 소환 이펙트
    public GameObject TrailObject;        // 플레이어의 공격을 표시할 Trail 오브젝트    
    public string CH_Name;                // 캐릭터 이름
    public int MP;                        // 캐릭터의 마나
    Vector3 startPos;                     // 플레이어의 시작 위치
    Quaternion rot;                       // 플레이어의 회전값
    public bool MainCharacter = false;    // 메인 캐릭터 여부

    protected override void Start()
    {
        base.Start();

        // 캐릭터의 이름을 통해 Scriptable에 세팅한 캐릭터의 데이터 세팅 
        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/Character/" + CH_Name));

        // 플레이어가 생성되었으니 Spawner에도 전달해주어야 한다.
        // 그래야 몬스터가 추적할 수 있는 대상이 될 수 있다.
        Spawner.m_Players.Add(this);

        Stage_Manager.m_ReadyEvent += OnReady;
        Stage_Manager.m_BossEvent += OnBoss;
        Stage_Manager.m_ClearEvent += OnClear;
        Stage_Manager.m_DeadEvent += OnDead;

        // 플레이어의 초기 시작 위치와 회전값을 저장
        // 몬스터를 추적하다가 몬스터가 범위 내에서 사라지면
        // 다시 제자리로 돌아오기 위한 변수
        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Data_Set(Character_Scriptable data)
    {
        CH_Data = data;
        Bullet_Name = data.m_Character_Name;
        Attack_Range = data.m_Attack_Range;
        ATK_Speed = data.m_Attack_Speed;

        Set_ATKHP();
    }

    public void Set_ATKHP()
    {
        ATK = Base_Manager.Player.Get_ATK(CH_Data.m_Rarity);
        HP = Base_Manager.Player.Get_HP(CH_Data.m_Rarity);
    }

    private void OnReady()
    {
        AnimatorChange("isIDLE");
        isDead = false;
        Spawner.m_Players.Add(this);
        Set_ATKHP();

        transform.position = startPos;
        transform.rotation = rot;
    }

    // 보스 스테이지 진입 시 모든 행동 IDLE화
    private void OnBoss()
    {
        AnimatorChange("isIDLE");
        Provocation_Effect.Play();
    }

    // 보스 클리어
    private void OnClear()
    {
        AnimatorChange("isCLEAR");
    }

    private void OnDead()
    {
        Spawner.m_Players.Add(this);
    }

    // MP 획득
    public void Get_MP(int mp)
    {
        // 스킬 사용중이라면 MP 획득 불가
        if (isGetSkill) return;
        // 메인 캐릭터는 MP 획득 불가
        if (MainCharacter) return;

        // 획득 MP 계산
        MP += mp;

        // 현재 MP가 최대 MP보다 높다면
        if (MP >= CH_Data.MaxMP)
        {
            MP = 0;
            // 현재 영웅의 SKillSet이 존재하면 스킬 발동
            if (GetComponent<Skill_Base>() != null)
            {
                GetComponent<Skill_Base>().SetSkill();
            }
            isGetSkill = true;
        }
        // MP 초기화
        Main_UI.instance.CharaterStateCheck(this);
    }

    private void Update()
    {
        // 죽을경우
        if (isDead) return;

        if (Stage_Manager.m_State == Stage_State.Play || Stage_Manager.m_State == Stage_State.Boss_Play)
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
            }
            else
            {
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

                    // 공격했을 시 MP 5 추가
                    Get_MP(5);

                    // 공격 후 공격 상태 초기화 함수 호출
                    // 공격 속도에 따른 공격 딜레이 조정
                    Invoke("InitAttack", 1.0f / ATK_Speed);
                }
            }
        }
    }

    // 캐릭터 넉백 연출 구현
    public void KnockBack()
    {
        StartCoroutine(Knockback_Coroutine(3.0f, 0.3f));
    }

    // 보스 소환 시 캐릭터 넉백 연출 구현을 위한 코루틴
    // power는 넉백의 세기, duration은 넉백 지속 시간
    IEnumerator Knockback_Coroutine(float power, float duration)
    {
        float t = duration;
        Vector3 force = this.transform.forward * -power;
        force.y = 0f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            if (Vector3.Distance(Vector3.zero, transform.position) < 3.0f)
            {
                transform.position += force * Time.deltaTime;
            }
            yield return null;
        }
    }

    // 공격 받은 후 이벤트 함수
    public override void GetDamage(double dmg)
    {
        base.GetDamage(dmg);

        if (Stage_Manager.isDead) return;

        // 공격 받았을 시 MP 3 추가
        Get_MP(3);

        var goObj = Base_Manager.Pool.Pooling_Obj("HIT_TEXT").Get((value) =>
        {
            value.transform.position = transform.position;
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, true);
        });

        HP -= dmg;
        
        if (HP <= 0)
        {
            isDead = true;
            DeadEvent();
        }
    }

    private void DeadEvent()
    {
        Spawner.m_Players.Remove(this);

        // 플레이어가 다 사망했다면 사망 상태로 전환
        if (Spawner.m_Players.Count <= 0 && Stage_Manager.isDead == false)
        {
            Stage_Manager.State_Change(Stage_State.Dead);
        }

        AnimatorChange("isDEAD");
        m_Target = null;
    }

    protected override void Attack()
    {
        base.Attack();
        TrailObject.SetActive(true);

        Invoke("TrailDisable", 1.0f);   
    }

    private void TrailDisable() => TrailObject.SetActive(false);
}
