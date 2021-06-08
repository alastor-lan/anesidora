using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoss : Monster
{
    public float strollCD;
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
        switch (monsterState)
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Stroll:
                Stroll();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Die:
                Die();
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
                monsterState = MonsterState.Stroll;
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
            monsterState = MonsterState.Attack;
            animator.SetBool("run", false);
        }
    }

    private void Idle()
    {
        if (isStart)
        {
            monsterState = MonsterState.Attack;
            animator.SetBool("run", true);
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
