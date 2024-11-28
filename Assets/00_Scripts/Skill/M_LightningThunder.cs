using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_LightningThunder : Skill_Base
{
    public override void SetSkill()
    {
        base.SetSkill();
        StartCoroutine(M_Skill_Coroutine());
    }

    // 보스몬스터 공격 스킬 루틴
    IEnumerator M_Skill_Coroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            Player player = players[Random.Range(0, players.Length)];
            Instantiate(Resources.Load("Pool_OBJ/Boss_Electric"), player.transform.position, Quaternion.identity);

            Camera_Manager.instance.CameraShake();

            player.GetDamage(10);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
