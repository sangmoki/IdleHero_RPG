using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Spawner : MonoBehaviour
{
    public Transform[] SpawnTransform = new Transform[6];
    public static Player[] players = new Player[6];

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SpawnTransform[i] = transform.GetChild(i);
        }

        Stage_Manager.m_ReadyEvent += OnReady;
    }

    private void OnReady()
    {
        for (int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Manager.Character.m_Set_Character[i];
            if (data != null)
            {
                // 영웅들이 이미 있다면 삭제하고 다시 생성
                if (players[i] != null)
                {
                    if (players[i].CH_Data != data.data)
                    {
                        Destroy(players[i].gameObject);
                        MakePlayer(data, i);
                    }
                }
                else
                {
                    MakePlayer(data, i);
                }
            }
        }
    }

    private void MakePlayer(Character_Holder data, int value)
    {
        string temp = data.data.m_Character_Name;
        var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));
        players[value] = go.GetComponent<Player>();
        go.transform.position = SpawnTransform[value].position;
        go.transform.LookAt(Vector3.zero);
    }
}
