using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Hero : MonoBehaviour
{
    public GameObject[] particles;  // 파티클
    public Transform[] Circles;     // 영웅의 위치

    public void GetParticle(bool m_B)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].SetActive(m_B);
        }
    }

    // 영웅 생성
    public void InitHero()
    {
        for (int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            // 캐릭터가 있을 경우
            if (Base_Manager.Character.m_Set_Character[i] != null)
            {
                // Resources 내의 오브젝트를 꺼내와 circle 위치에 생성한다.
                string temp = Base_Manager.Character.m_Set_Character[i].data.m_Character_Name;
                var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));
                go.transform.position = Circles[i].position;
            }
        }
    }
}
