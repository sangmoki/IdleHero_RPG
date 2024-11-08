using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;

    // virtual키워드는 - 상속받은 내부에서 override 사용이 가능하다.
    // 즉, 상속받은 함수 내부에서 추가 수정이 가능하다.
    protected virtual void Start()
    {
        // animator 객체에 컴포넌트 할당
        animator = GetComponent<Animator>();
    }

    // 동작 변경 함수
    protected void AnimatorChange(string temp)
    {
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }
}
