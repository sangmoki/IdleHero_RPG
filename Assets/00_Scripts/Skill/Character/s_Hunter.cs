using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Hunter : Skill_Base
{
    public override void SetSkill()
    {
        base.SetSkill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    // 5초 지속 공격속도 2배 상승 스킬
    IEnumerator Set_Skill_Coroutine()
    {
        // 스킬 발동 - 공격속도 2배 상승 (5초 지속)
        m_Player.ATK_Speed = 2.0f;
        SkillParticle.SetActive(true);
        yield return new WaitForSeconds(5.0f);

        // 스킬 발동 후 이펙트 및 공격속도 초기화
        m_Player.ATK_Speed = 1.0f;
        SkillParticle.SetActive(false);
        ReturnSkill();
    }
}
