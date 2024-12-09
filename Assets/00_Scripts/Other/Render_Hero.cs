using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Hero : MonoBehaviour
{
    public Transform[] Circles;     // 영웅의 위치
    public Transform Pivot;         // 중심점
    public GameObject[] particles;  // 파티클
    public bool[] GetCharacter = new bool[6];

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
            if (Base_Manager.Character.m_Set_Character[i] != null && GetCharacter[i] == false)
            {
                // Resources 내의 오브젝트를 꺼내와 circle 위치에 생성한다.
                string temp = Base_Manager.Character.m_Set_Character[i].data.m_Character_Name;
                var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));

                // 레이어 변경
                for (int j = 0; j < go.transform.childCount; j++)
                {
                    go.transform.GetChild(j).gameObject.layer = LayerMask.NameToLayer("Render_Layer");
                }
                // 부모 설정
                go.transform.parent = transform;
                // Player 컴포넌트를 비활성화
                go.GetComponent<Player>().enabled = false;
                // 위치 설정
                go.transform.position = Circles[i].position;
                // Pivot을 바라보게 한다.
                go.transform.LookAt(Pivot.position);
            }
        }
    }
}
