using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Spawner : MonoBehaviour
{
    public Transform[] SpawnTransform = new Transform[6];

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
                string temp = data.data.m_Character_Name;
                var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));
                go.transform.position = SpawnTransform[i].position;
                go.transform.LookAt(Vector3.zero);
            }
        }
    }
}
