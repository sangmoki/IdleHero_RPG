using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;   // 총알 속도
    Transform m_Target;      // 타겟
    Vector3 m_TargetPos;     // 타겟의 위치
    double m_DMG;            // 데미지
    string m_Character_Name; // Resources 내의 오브젝트 이름
    bool GetHit = false;     // 타겟을 맞았는지 확인 - 머즐이 여러번 중복되는 것 방지

    Dictionary<string, GameObject> m_Projectiles = new Dictionary<string, GameObject>();
    Dictionary<string, ParticleSystem> m_Muzzles = new Dictionary<string, ParticleSystem>();

    // 생성이 되자마자 자식들을 꺼내온다.
    private void Awake()
    {
        // GetChild(index) - 순서대로 자식 오브젝트를 가져온다.
        Transform projectiles = transform.GetChild(0);
        Transform muzzles = transform.GetChild(1);

        // 반복을 통해 자식 오브젝트를 Dictionary에 저장
        for (int i = 0; i < projectiles.childCount; i++) 
        {
            m_Projectiles.Add(projectiles.GetChild(i).name, projectiles.GetChild(i).gameObject);
        }

        for (int i = 0; i < muzzles.childCount; i++)
        {
            m_Muzzles.Add(muzzles.GetChild(i).name, muzzles.GetChild(i).GetComponent<ParticleSystem>());
        }
    }

    public void Init(Transform target, double dmg, string Character_Name)
    {
        m_Target = target;
        transform.LookAt(m_Target);
        GetHit = false;
        m_TargetPos = m_Target.position;
        m_DMG = dmg;
        m_Character_Name = Character_Name;
        // 총알 발사 이펙트 활성화
        m_Projectiles[m_Character_Name].gameObject.SetActive(true);
    }

    private void Update()
    {
        // 맞은 상태라면 return
        if (GetHit) return;
        m_TargetPos.y = 0.5f; // 총알 높이 조정

        // 총알을 타겟의 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, Time.deltaTime * m_Speed);

        // 총알과 타겟의 거리가 0.1f보다 작을 경우
        if (Vector3.Distance(transform.position, m_TargetPos) <= 0.1f)
        {
            if (m_Target != null)
            {
                GetHit = true;
                // 타겟의 HP를 데미지만큼 감소
                m_Target.GetComponent<Monster>().GetDamage(10);
                //m_Target.GetComponent<Character>().HP -= m_DMG;
                // 총알 발사 이펙트 비활성화
                m_Projectiles[m_Character_Name].gameObject.SetActive(false);
                // 머즐 활성화
                m_Muzzles[m_Character_Name].Play();

                //  Muzzle의 이펙트가 살아있는 시간동안 기다린 후 총알 반환
                StartCoroutine(ReturnObject(m_Muzzles[m_Character_Name].duration));
            }
        }
    }

    // 불렛 리턴 함수
    IEnumerator ReturnObject(float timer)
    {
        yield return new WaitForSeconds(timer);
        Base_Mng.Pool.m_pool_Dictionary["Bullet"].Return(this.gameObject);
    }
}
