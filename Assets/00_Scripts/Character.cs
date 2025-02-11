using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;

    public double HP;                    // 체력
    public double ATK;                   // 공격력
    public float ATK_Speed;              // 공격 속도
    public bool isDead = false;          // 사망 상태 플래그
    protected float Attack_Range = 3.0f; // 공격 범위
    protected float target_Range = 5.0f; // 타겟을 공격할 수 있는 인지 범위 
    protected bool isATTACK = false;     // 공격 상태
    public bool isGetSkill = false;      // 스킬 획득 여부
    public bool SkillNoneAttack = false; // 스킬 공격 여부

    public Transform m_Target;           // 타겟

    [SerializeField]
    public Transform m_BulletTransform;  // 총알 생성 위치

    public string Bullet_Name;           // 총알 이름

    // virtual키워드는 - 상속받은 내부에서 override 사용이 가능하다.
    // 즉, 상속받은 함수 내부에서 추가 수정이 가능하다.
    protected virtual void Start()
    {
        // animator 객체에 컴포넌트 할당
        animator = GetComponent<Animator>();
    }

    // 공격 상태 초기화 함수
    protected void InitAttack() => isATTACK = false;

    // 동작 변경 함수
    public void AnimatorChange(string temp)
    {
        // 버프 형태가 아닌 공격 형태의 스킬일 경우에만 실행
        if (SkillNoneAttack)
        {
            // 스킬이 발동 중 일때는 Animator를 변경하지 않는다.
            if (isGetSkill) return;
        }


        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        if (temp == "isATTACK" || temp == "isCLEAR" || temp == "isDEAD" || temp == "isSKILL")
        {
            if (temp == "isATTACK")
            {
                animator.speed = ATK_Speed;
            }
            animator.SetTrigger(temp);
            return;
        }
        animator.speed = 1.0f;
        animator.SetBool(temp, true);
    }

    // 원거리 공격 함수
    protected virtual void Bullet()
    {
        // 타겟이 없다면 불릿이 생성되지 않게 리턴
        if (m_Target == null) return;

        // 생성된 Bullet의 위치는 m_BulletTransform의 위치로 설정
        Base_Manager.Pool.Pooling_Obj("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().Init(m_Target, ATK, Bullet_Name);
        });
    }

    // 근접 공격 함수
    protected virtual void Attack()
    {
        if (target_Range == null) return;

        Base_Manager.Pool.Pooling_Obj("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_Target.position;
            value.GetComponent<Bullet>().Attack_Init(m_Target, ATK);
        });
    }

    // 공격 받은 후 이벤트 함수
    public virtual void GetDamage(double dmg)
    {
        //HP -= dmg;
        //if (HP <= 0)
        //{
        //    isDead = true;
        //    animator.SetTrigger("isDEAD");
        //}
    }

    // 힐 스킬 사용 시 이벤트
    public virtual void Heal(double heal)
    {
        // 현재 체력에 힐량을 더한다.
        HP += heal;

        // 힐량 텍스트 생성
        var goObj = Base_Manager.Pool.Pooling_Obj("HIT_TEXT").Get((value) =>
        {
            value.transform.position = transform.position;
            value.GetComponent<HIT_TEXT>().Init(transform.position, heal, Color.green, true);
        });
    }

    // 타겟을 공격할 수 있는 인지 범위(추적)
    protected void FindClosetTarget<T>(T[] targets) where T : Component
    {
        var monsters = targets;
        // 가장 가까운 몬스터를 찾기 위한 변수
        Transform closetTarget = null;
        // 타겟 공격 인지 범위 - 만약 5.0f라면 5.0f 이내의 몬스터를 찾는다.
        float maxDistance = target_Range;

        // 반복문을 통해 가장 가까운 몬스터와의 거리 계산
        foreach (var monster in monsters)
        {
            // 전달받은 객체와 몬스터의 거리를 계산
            float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

            // 타겟의 범위가 공격 인지 범위보다 작다면
            if (targetDistance < maxDistance)
            {
                // 타겟 지정
                closetTarget = monster.transform;
                maxDistance = targetDistance;
            }
            m_Target = closetTarget;

            // 만약 타겟이 지정되면 타겟의 위치를 바라보는 상태로 변경
            if (m_Target != null) transform.LookAt(m_Target.position);
        }

    }
}
