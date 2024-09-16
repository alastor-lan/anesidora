using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BossState
{
    Idle,
    Stroll,
    Second_Idle,
    Attack,
    Die,
    Pure
}
public class Boss : MonoBehaviour, BeAttack
{
    public float hp;
    public float currentHP;
    public int coin;
    public int magic;
    protected string role = "Monster";
    public bool isStart = false;
    protected Transform targetPosition;
    protected Room room;
    protected BossState bossState;

    public virtual void BeAttack(float data)
    {
             hp -= data;
        if(hp>=currentHP/2)
        {
            bossState = BossState.Idle;
            GetComponent<Animator>().SetBool("isFirst", true);
            
        }
        if(hp<=currentHP/2)
        {
            bossState = BossState.Second_Idle;
            GetComponent<Animator>().SetBool("isHenshin", true);
            GetComponent<Animator>().SetBool("isFirst", false);
            GetComponent<Animator>().SetBool("isSecond", true);
        }
        if (hp <= 0)
        {
            bossState = BossState.Die;
            GetComponent<Animator>().SetBool("isDie", true);
            GetComponent<Collider2D>().enabled = false;
            room.BossDie(this);
            //Invoke("Die", 1.0f);
            for (int i = 0; i < coin; i++)
            {
                Instantiate(GameManager.instance.coinPre, transform.position, Quaternion.identity);
            }
            for (int i = 0; i < magic; i++)
            {
                Instantiate(GameManager.instance.mpPre, transform.position, Quaternion.identity);
            }
        }
        else
        {
            GetComponent<Animator>().Play("BeAttack");
            GameManager.instance.ShowAttack(data, Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0)));
        }
    }
    void Die()
    {
        Destroy(gameObject);

    }
    public virtual void Initialization(Room room)
    {
        this.room = room;
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
