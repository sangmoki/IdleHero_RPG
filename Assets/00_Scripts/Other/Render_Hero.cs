using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Hero : MonoBehaviour
{
    public GameObject[] particles;

    public void GetParticle(bool m_B)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].SetActive(m_B);
        }
    }
}
