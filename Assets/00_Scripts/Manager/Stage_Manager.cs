using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// delegate chain
// 하나의 delegate가 여러 함수를 참조할 수 있다.
// 이벤트 발생 시 호출할 함수의 형태를 미리 정의
public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();


public class Stage_Manager
{
    // 상태 패턴(State Pattern)
    // 객체 상태에 따라 어떤 행동을 할 것 인가에 대한 디자인 패턴
    public static Stage_State m_State;

    public static int MaxCount = 5;     // 보스에 도달할 최대 몬스터 수
    public static int Count;        // 현재 몬스터 수

    public static OnReadyEvent m_ReadyEvent;
    public static OnPlayEvent m_PlayEvent;
    public static OnBossEvent m_BossEvent;
    public static OnClearEvent m_ClearEvent;
    public static OnDeadEvent m_DeadEvent;

    // 스테이지에 따른 상태 변경
    public static void State_Change(Stage_State state)
    {
        m_State = state;
        switch (state)
        {
            case Stage_State.Ready:
                Debug.Log("isReady!");
                m_ReadyEvent?.Invoke();
                Base_Mng.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play:
                Debug.Log("isPlay!");
                m_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
                Debug.Log("isBoss!");
                m_BossEvent?.Invoke();
                break;
            case Stage_State.Clear:
                Debug.Log("isClear!");
                m_ClearEvent?.Invoke();
                break;
            case Stage_State.Dead:
                Debug.Log("isDead!");
                m_DeadEvent?.Invoke();
                break;
        }
    }

}
