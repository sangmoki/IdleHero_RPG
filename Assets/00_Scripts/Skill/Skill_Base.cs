using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    protected Monster[] monsters {  get { return Spawner.m_Monsters.ToArray(); } }
    protected Player[] players { get { return Spawner.m_Players.ToArray(); } }

    private void Start()
    {
        Stage_Manager.m_DeadEvent += OnDead;
    }

    public virtual void SetSkill()
    {

    }

    private void  OnDead()
    {
        StopAllCoroutines();
    }
}
