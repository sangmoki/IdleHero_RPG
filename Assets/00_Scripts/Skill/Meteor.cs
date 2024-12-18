using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float speed;

    public ParticleSystem Explosion_Particle;
    public Transform Meteor_OBJ;
    public Transform Circle;

    Transform parentTransform;

    public void Init(double dmg)
    {
        if (parentTransform == null)
        {
            parentTransform = transform.parent;
        }
        transform.parent = null;
        StartCoroutine(Meteor_Coroutine(dmg));
    }

    // 메테오 스킬 구현
    IEnumerator Meteor_Coroutine(double dmg)
    {
        // 랜덤한 위치
        Meteor_OBJ.localPosition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(10.0f, 15.0f), Random.Range(5.0f, 10.0f));
        Meteor_OBJ.gameObject.SetActive(true);
        Meteor_OBJ.LookAt(transform.parent);

        Circle.localScale = Vector3.one;
        SpriteRenderer renderer = Circle.GetComponent<SpriteRenderer>();

        while (true)
        {
            // 랜덤한 위치 추적
            float distance = Vector3.Distance(Meteor_OBJ.localPosition, Vector3.zero);
            if (distance >= 0.1f)
            {
                Meteor_OBJ.localPosition = Vector3.MoveTowards(Meteor_OBJ.localPosition, Vector3.zero, speed * Time.deltaTime);
                // 땅에 가까워질수록 연하게 
                float scaleValue = distance / speed + 0.3f;
                renderer.color = new Color(0, 0, 0, Mathf.Min((distance / speed), 0.5f));
                Circle.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                yield return null;
            }
            else
            {
                Explosion_Particle.Play();
                Camera_Manager.instance.CameraShake();

                // 현재 화면상의 몬스터 리스트를 가져와
                for (int i = 0; i < Spawner.m_Monsters.Count; i++)
                {
                    // 만약 몬스터가 1.5f 이내에 있다면 데미지를 입힌다.
                    if (Vector3.Distance(transform.position, Spawner.m_Monsters[i].transform.position) <= 1.5f)
                    {
                        Spawner.m_Monsters[i].GetDamage(dmg);
                    }
                }
                break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        transform.parent = parentTransform;
        Meteor_OBJ.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
