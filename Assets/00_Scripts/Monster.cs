using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // ������ ���ǵ�
    public float m_Speed;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        // ���Ͱ� �ɾ�� ������ �Ĵٺ���.
        transform.LookAt(Vector3.zero);

        // Distance�� ù��° ���ڿ� �ι�° ������ ���հ�
        // transform.position : ������ ���� ��ġ
        // Vector3.zero : (0, 0, 0) - ��� ��ġ
        // ��, targetDistance = transform.position�� (0, 0, 0) ������ �Ÿ�
        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);
        if (targetDistance <= 0.5f)
        {
            // ���Ͱ� ����� �����ϸ� IDLE ���·� ����
            AnimatorChange("isIDLE");
        }
        else
        {
            // ���Ͱ� (0, 0, 0) - ����� �̵�
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * m_Speed);
            // ���Ͱ� ����� �̵����̸� MOVE ���·� ����
            AnimatorChange("isMOVE");
        }
    }

    // ������ ���� ���� �Լ�
    private void AnimatorChange(string temp)
    {
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }
}
