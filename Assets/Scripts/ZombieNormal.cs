using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ZombieNormal : MonoBehaviour
{
    //僵尸移动方向
    public Vector3 direction = new Vector3(-1, 0, 0);
    public float speed = 10;

    //动画
    private Animator animator;
    private bool isWalk;

    //攻击与受伤
    public float damage;
    public float damageInterval = 0.5f;
    private float damageTimer;

    [Header("基础参数")]
    public float health = 100;
    //表示僵尸血量到达多少时掉头
    public float lostHeadHealth = 50;
    private float currentHealth;
    private GameObject head;
    private bool lostHead;
    private bool isDie;


    void Start()
    {
        isWalk = true;
        animator = GetComponent<Animator>();
        damageTimer = 0;

        currentHealth = health;
        head = transform.Find("Head").gameObject;
        lostHead = false;
        isDie = false;
    }

    void Update()
    {
        if(isDie)
            { return;   }
        Move();
    }

    private void Move()
    {
        if(isWalk)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isDie)
            { return;   }
        if(other.tag == "Plant")
        {
            //碰到植物，停止移动
            isWalk = false;
            animator.SetBool("Walk", false);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(isDie)
            { return;   }
        if(other.tag == "Plant")
        {
            //持续接触植物，造成伤害
            damageTimer += Time.deltaTime;
            if(damageTimer >= damageInterval)
            {
                damageTimer  = 0;
                //对植物造成伤害
                Plant plant =  other.GetComponent<Plant>();
                float newHealth =  plant.ChangeHealth(-damage);
                if(newHealth <= 0)
                {
                    isWalk = true;
                    animator.SetBool("Walk", true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(isDie)
            { return;  }
        if(other.tag == "Plant")
        {
            isWalk = true;
            animator.SetBool("Walk", true);
        }
    }

    //僵尸受击
    public void ChangeHealth(float num)
    {
        //僵尸受到攻击
        currentHealth = Mathf.Clamp(currentHealth + num,0,health);
        if(currentHealth < lostHeadHealth && !lostHead)
        {
            lostHead = true;
            animator.SetBool("LostHead",true);
            head.SetActive(true);
        }
        if(currentHealth <= 0)
        {
            animator.SetTrigger("Die");
            isDie = true;
        }
    }

    public void DieAniOver()
    {
        animator.enabled = false;
        GameObject.Destroy(gameObject);
    }
}
