using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float m_Speed;   // ������ �̵��ӵ�
    Animator animator;

    bool isSpawn = false;   // ���� Ȯ�� �÷���

    private void Start()
    {
        // animator ��ü�� ������Ʈ �Ҵ�
        animator = GetComponent<Animator>();
        // ���Ͱ� ������ �� ũ�Ⱑ ���� Ŀ���� ȿ���� �ֱ� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine(Spawn_Start());
    }

    // ���Ͱ� ������ �� �߻��ϴ� �ڷ�ƾ �Լ�
    // ���Ͱ� ������ �� ũ�Ⱑ ���� Ŀ���� ȿ�� - ������ �� �� �ڿ������� �ϱ� ����
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x;

        // percent�� 1���� ���� ���� ����
        while (percent < 1)
        {
            current += Time.deltaTime;  // 1�ʿ� �ɸ��� �ð�
            percent = current / 0.2f;   // 1�ʿ� �ɸ��� �ð��� 1�� ������ 0~1 ������ ������ ��ȯ

            // ���۰�(0.0f)���� ����(����.x) ������ �ɸ��� Ư�� �ð� �ӵ��� �̵�
            float LerpPos = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }

    private void Update()
    {
        // ���Ͱ� �ɾ�� ������ �Ĵٺ���.
        transform.LookAt(Vector3.zero);

        // isSpawn�� false�̸� return
        if (isSpawn == false) return;

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
