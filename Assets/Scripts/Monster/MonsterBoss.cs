using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoss : Boss
{
    public float strollCD;
    public bool isPure;
    public float attackCD = 0.5f;
    public Weapon weapon;
    Animator animator;
    MyAstarAI myAI;
    float strolltiming;
    float attacktiming;
    public int MaxHP;
    private void Start()
    {
        myAI = GetComponent<MyAstarAI>();
        animator = GetComponent<Animator>();
        //注册
        weapon.role = role;
    }
    void Update()
    {
        switch (bossState)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Stroll:
                Stroll();
                break;
            case BossState.Attack:
                Attack();
                break;
            case BossState.Die:
                Die();
                break;
            case BossState.Pure:
                Pure();
                break;
            case BossState.Second_Idle:
                Second_Idle();
                break;
            default:
                break;
        }
    }

    private void Die()
    {
        
    }

    public void UpdateLookAt(Vector3 targetPos)
    {
        if (transform.position.x > targetPos.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (transform.position.x < targetPos.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        weapon.UpdateLookAt(targetPos);
    }

    void Attack()
    {
        UpdateLookAt(targetPosition.position);
        if (Time.time - attacktiming >= attackCD)
        {
            attacktiming = Time.time;
            int rand = UnityEngine.Random.Range(0, 5);
            if (rand ==1)
            {
                bossState = BossState.Stroll;
                animator.SetBool("run", true);
            }
            else
            {
                weapon.ShootButtonDown();
            }
        }
    }
    private void Stroll()
    {
        if (Time.time - strolltiming >= strollCD)
        {
            strolltiming = Time.time;
            myAI.RandomPath();
        }
        myAI.NextTarget();
        UpdateLookAt(targetPosition.position);
        if (myAI.reachedEndOfPath)
        {
            bossState = BossState.Attack;
            animator.SetBool("run", false);
        }
    }

    private void Idle()
    {
        if (isStart)
        {
            bossState = BossState.Attack;
            animator.SetBool("run", true);
        }
    }
    private void Pure()
    {
        if (isPure)
        {
            bossState = BossState.Pure;
            animator.SetBool("isNecklace", true);
        }
    }
    private void Second_Idle()
    {
        if(hp<=currentHP)
        {
            Time.timeScale = 0;
            bossState = BossState.Attack;
            animator.SetBool("isHinshin", true);
            animator.SetBool("isSecond", true);
            animator.SetBool("run", true);
            if (bossState == BossState.Attack)
            { Time.timeScale = 1; }
        }
    }
    public override void BeAttack(float data)
    {
        base.BeAttack(data);
        if (hp <= 0)
        {
            weapon.gameObject.SetActive(false);

        }
    }
}
