using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character : Skill_Base
{
    float delay = 5.0f;

    Coroutine coroutine = null;

    private void Start()
    {
        Stage_Manager.m_ReadyEvent += OnReady;
    }

    public override void SetSkill()
    {
        // 스킬 사용 애니메이션 실행
        m_Player.isGetSkill = true;
        m_Player.AnimatorChange("isSKILL");

        // 가장 체력이 적은 타겟을 찾는다.
        var character = HP_Check();

        // 찾은 타겟에게 힐 스킬 사용
        m_Player.transform.LookAt(character.transform.position);
        character.Heal(SkillDamage(110));
        SkillParticle.gameObject.SetActive(true);
        SkillParticle.transform.position = character.transform.position;

        Invoke("ReturnSkill", 1.0f);
        base.SetSkill();
    }

    public override void ReturnSkill()
    {
        OnReady();
        SkillParticle.gameObject.SetActive(false);  
        base.ReturnSkill();
    }

    public void OnReady()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SkillCoroutine(delay));
    }

    IEnumerator SkillCoroutine(float value)
    {
        float timer = value;
        while(timer > 0.0f)
        { 
            timer -= Time.deltaTime;
            Main_UI.instance.Main_Character_Skill_Fill.fillAmount = timer / value;
            yield return null;
        }
        SetSkill();
    }
}
