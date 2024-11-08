using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;

    // virtualŰ����� - ��ӹ��� ���ο��� override ����� �����ϴ�.
    // ��, ��ӹ��� �Լ� ���ο��� �߰� ������ �����ϴ�.
    protected virtual void Start()
    {
        // animator ��ü�� ������Ʈ �Ҵ�
        animator = GetComponent<Animator>();
    }

    // ���� ���� �Լ�
    protected void AnimatorChange(string temp)
    {
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }
}
