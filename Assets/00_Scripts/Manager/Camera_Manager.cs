using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    float m_Distance = 4.0f;   // 카메라와의 거리

    // 카메라와의 거리를 조절하기 위한 기준치 변수
    [Range(0.0f, 10.0f)]
    [SerializeField] private float Distance_Value;  

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        // 카메라를 가장 멀리 있는 캐릭터의 위치로 deltaTime * 2.0 만큼의 속도로 이동
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Distance(), Time.deltaTime * 2.0f);
    
    }

    // 가장 멀리 있는 플레이어의 위치 계산
    float Distance()
    {
        // 카메라와 플레이어의 거리를 계산하여
        // 가장 멀리있는 플레이어 기준으로 카메라와의 거리를 설정
        var players = Spawner.m_Players.ToArray();
        float maxDistance = m_Distance;

        foreach(var player in players)
        {
            float targetDistance = Vector3.Distance(Vector3.zero, player.transform.position) + Distance_Value;

            if (targetDistance > maxDistance)
                maxDistance = targetDistance;
        }

        return maxDistance;
    }
}
