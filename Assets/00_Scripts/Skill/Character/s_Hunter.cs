using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Hunter : Skill_Base
{
    public override void SetSkill()
    {
        base.SetSkill();
    }

    // 5초 지속 공격속도 2배 상승 스킬
    IEnumerator Set_Skill_Coroutine()
    {
        m_Player.ATK_Speed = 2.0f;
        SkillParticle.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        ReturnSkill();
    }
}
