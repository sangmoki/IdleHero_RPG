using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;

    public double HP;                    // ü��
    public double ATK;                   // ���ݷ�
    public float ATK_Speed;              // ���� �ӵ�
    protected float Attack_Range = 1.0f; // ���� ����
    protected float Target_Range = 3.0f; // Ÿ���� ������ �� �ִ� ���� ���� 
    protected bool isATTACK = false;     // ���� ����

    protected Transform m_Target;        // Ÿ��

    [SerializeField]
    public Transform m_BulletTransform; // �Ѿ� ���� ��ġ

    // virtualŰ����� - ��ӹ��� ���ο��� override ����� �����ϴ�.
    // ��, ��ӹ��� �Լ� ���ο��� �߰� ������ �����ϴ�.
    protected virtual void Start()
    {
        // animator ��ü�� ������Ʈ �Ҵ�
        animator = GetComponent<Animator>();
    }

    // ���� ���� �ʱ�ȭ �Լ�
    protected void InitAttack() => isATTACK = false;

    // ���� ���� �Լ�
    protected void AnimatorChange(string temp)
    {
        if (temp == "isATTACK")
        {
            animator.SetTrigger("isATTACK");
            return;
        }

        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }

    protected virtual void Bullet()
    {
        // ������ Bullet�� ��ġ�� m_BulletTransform�� ��ġ�� ����
        Base_Mng.Pool.Pooling_Obj("Bullet").Get((value) =>
        {
            value.transform.position = m_BulletTransform.position;
        });
    }

    // Ÿ���� ������ �� �ִ� ���� ����(����)
    protected void FindClosetTarget<T>(T[] targets) where T : Component
    {
        var monsters = targets;
        // ���� ����� ���͸� ã�� ���� ����
        Transform closetTarget = null;
        // Ÿ�� ���� ���� ���� - ���� 5.0f��� 5.0f �̳��� ���͸� ã�´�.
        float maxDistance = Target_Range;

        // �ݺ����� ���� ���� ����� ���Ϳ��� �Ÿ� ���
        foreach (var monster in monsters)
        {
            // ���޹��� ��ü�� ������ �Ÿ��� ���
            float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

            // Ÿ���� ������ ���� ���� �������� �۴ٸ�
            if (targetDistance < maxDistance)
            {
                // Ÿ�� ����
                closetTarget = monster.transform;
                maxDistance = targetDistance;
            }
            m_Target = closetTarget;

            // ���� Ÿ���� �����Ǹ� Ÿ���� ��ġ�� �ٶ󺸴� ���·� ����
            if(m_Target != null) transform.LookAt(m_Target.position);
        }

    }
}
