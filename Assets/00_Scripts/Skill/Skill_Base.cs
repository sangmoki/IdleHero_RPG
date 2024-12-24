using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_Base : MonoBehaviour
{

    public GameObject SkillParticle;
    protected Monster[] monsters {  get { return Spawner.m_Monsters.ToArray(); } }
    protected Player[] players { get { return Spawner.m_Players.ToArray(); } }
    protected Character m_Player { get { return GetComponent<Character>(); } }

    private void Start()
    {
        Stage_Manager.m_DeadEvent += OnDead;
    }

    public virtual void SetSkill()
    {

    }

    // 스킬 데미지 계산 함수 = 캐릭터 공격력 * (스킬 데미지 / 100)
    // 만약 스킬 데미지가 130% 라면 1.3f로 계산
    // 즉, 플레이어 공격력 * 1.3f
    protected double SkillDamage(double value)
    {
        return m_Player.ATK * (value / 100.0f);
    }

    // 몬스터와의 거리 계산 플래그 함수
    protected bool Distance(Vector3 startPos, Vector3 endPos, float distanceValue)
    {
        return Vector3.Distance(startPos, endPos) <= distanceValue;
    }

    // 캐릭터가 사망했을 때 호출되는 함수
    private void  OnDead()
    {
        StopAllCoroutines();
    }

    // 스킬을 반환하고 Idle 상태로 전환
    public virtual void ReturnSkill()
    {
        m_Player.isGetSkill = false;
        m_Player.AnimatorChange("isIDLE");
    }

    // HP가 가장 낮은 영웅을 찾는 함수
    public Character HP_Check()
    {
        Character player = null;

        double hpCount = Mathf.Infinity;

        for (int i = 0; i < players.Length; i++)
        {
            double hp = players[i].HP;

            if (hp < hpCount)
            {
                hpCount = hp;
                player = players[i];
            }
        }

        return player;
    }
}
