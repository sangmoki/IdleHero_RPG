using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Elementalist_W : Skill_Base
{
    public override void SetSkill()
    {
        base.SetSkill();

        StartCoroutine(Set_Skill_Coroutine());
    }

    // 스킬 발동
    IEnumerator Set_Skill_Coroutine()
    {
        SkillParticle.SetActive(true);
        int value = SkillParticle.transform.childCount;

        for (int i = 0; i < value; i++)
        {
            var meteor = SkillParticle.transform.GetChild(i).GetComponent<Meteor>();
            meteor.gameObject.SetActive(true);
            Vector3 pos = monsters[Random.Range(0, monsters.Length)].transform.position 
                + new Vector3(Random.insideUnitSphere.x * 3.0f
                , 0.0f
                , Random.insideUnitSphere.z * 3.0f);

            meteor.transform.position = pos;
            meteor.Init(SkillDamage(150.0f));
            yield return new WaitForSeconds(0.2f);
        }
        ReturnSkill();
    }
}
