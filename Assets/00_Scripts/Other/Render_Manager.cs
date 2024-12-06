using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Manager : MonoBehaviour
{
    public static Render_Manager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // RenderOBJ의 카메라
    public Camera cam;

    public Render_Hero HERO;
    
    public Vector2 ReturnScreenPoint(Transform pos)
    {
        // 월드좌표를 스크린좌표로 변경
        return cam.WorldToScreenPoint(pos.position);
    }
}
