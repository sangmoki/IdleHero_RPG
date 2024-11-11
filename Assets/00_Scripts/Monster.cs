using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public float m_Speed;   // ������ �̵��ӵ�

    bool isSpawn = false;   // ���� Ȯ�� �÷���

    // �ʱⰪ ����
    protected override void Start()
    {
        base.Start();
        HP = 5;
    }

    // ��Ȱ�� - Start���� �����ϸ� �ѹ� ������Ʈ�� ������ ������ ������ �ȵ�.
    public void Init()
    {
        // Ǯ������ ���Ͱ� �ʱ�ȭ �� �� ������ ���� �ʱ�ȭ
        isDead = false;
        HP = 5;

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

    // ������ �ǰ� �Լ�
    public void GetDamage(double dmg)
    {
        // ���Ͱ� �̹� �׾������� return
        if (isDead) return;

        // ���Ͱ� �ǰݴ����� ��� HitText�� Ǯ������ �����ͼ� �ǰݴ��� �ؽ�Ʈ�� ����
        Base_Mng.Pool.Pooling_Obj("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg);
        });

        HP -= dmg;
        
        // ������ ü���� 0���ϰ� �Ǹ� ������ ����
        if (HP <= 0)
        {
            isDead = true;
            // ���͸� ã�� ���ϰ� �ϱ� ���� ���� ����Ʈ���� ����
            Spawner.m_Monsters.Remove(this);

            // ����ũ ����Ʈ �ο�
            var smokeObj = Base_Mng.Pool.Pooling_Obj("Smoke").Get((value) =>
            {
                value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                // ����ũ ����Ʈ�� ������ ��ȯ
                Base_Mng.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");
            });

            // ���͸� Ǯ������ ��ȯ
            Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
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



}
