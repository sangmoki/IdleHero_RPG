using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class s_Barbarian : Skill_Base
{
    public override void SetSkill()
    {
        m_Player.AnimatorChange("isSKILL");
        SkillParticle.SetActive(true);
        StartCoroutine(Set_Skill_Coroutine());
        base.SetSkill();
    }

    public override void ReturnSkill()
    {
        SkillParticle.SetActive(false);
        base.ReturnSkill();
    }

    // 휠윈드 스킬
    IEnumerator Set_Skill_Coroutine()
    {
        // 휠윈드는 5타수
        for (int i = 0; i < 5; i++)
        {
            // 몬스터와의 거리가 1.5f 이하라면 몬스터 공격
            for (int j = 0; j < monsters.Count(); j++)
            {
               if (Distance(transform.position, monsters[i].transform.position, 1.5f)) 
               {
                    // 플레이어 공격력의 130%의 데미지를 입힌다.
                    monsters[i].GetDamage(SkillDamage(130));
               }
            }
            yield return new WaitForSeconds(0.5f);
        }
        ReturnSkill();
    }
}
