using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[Serializable]
public class SpecificValue
{
    public float realValue=10;
    public float maxValue=10;
    public float specificValue
    {
        get { return realValue / maxValue; }
    }
}
public class PlayerControl : MonoBehaviour, BeAttack
{
    public static PlayerControl instance;
    public SpecificValue playerHP;
    public SpecificValue playerDP;
    public SpecificValue playerMP;
    public int coin;
    //move
    public float moveSpeed = 5f;
    public float dpCD1 = 1f;
    public float dpCD2 = 0.5f;

    public GameObject playerPrefab;
    public bool isDie;
    private Rigidbody2D myRigidbody;
    private Weapon weapon;
    private Animator playerAnima;
    GameObject myPlayer;
    GameObject myWeapon;
    Vector2 movement;
    Vector2 target;

    bool fireKeyDown;
    bool fireKeyUp;
    bool fireKeyPressed;
    GameObject weaponInFloor;
    List<GameObject> nearWeapons = new List<GameObject>();
    float timing;
    float dpTiming1;
    float dpTiming2;
    bool dpRestore = false;
    private void Awake()
    {
          instance = this;

    }

  private void Start()
    {
        myPlayer = Instantiate(playerPrefab, transform);
        playerAnima = myPlayer.GetComponent<Animator>();
        myRigidbody = transform.GetComponent<Rigidbody2D>();
        isDie = false;
        if(playerHP.maxValue > 0)
        {
           DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    GameManager.instance.ShowAttack(1, Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0)));    
        //}
        fireKeyDown = Input.GetMouseButtonDown(0);
        fireKeyUp = Input.GetMouseButtonUp(0);
        if (fireKeyDown)
        {
            timing = 0;
        }
        if (Input.GetMouseButton(0))
        {
            timing += Time.deltaTime;
        }
        if (fireKeyUp)
        {
            timing = 0;
        }
        if (timing >= 0.2)
        {
            fireKeyPressed = true;
        }
        else
        {
            fireKeyPressed = false;
        }
        if (!isDie)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            SetLookAt();
            if (movement == new Vector2(0, 0))
            {
                playerAnima.SetBool("run", false);
            }
            else
            {
                playerAnima.SetBool("run", true);
            }
            Ray2DForCircle();
            if (weaponInFloor != null && fireKeyDown)
            {
                GetWeapon();
            }
            else
            {
                if (fireKeyDown)
                {
                    if (weapon != null)
                    {
                        weapon.ShootButtonDown();
                    }
                    else
                    {
                        //Debug.Log("没有武器只能手刀撒");
                    }
                }
                else if (fireKeyPressed)
                {
                    if (weapon != null)
                    {
                        weapon.ShootButtonPressed();
                    }
                }
                if (fireKeyUp)
                {
                    if (weapon != null)
                    {
                        weapon.ShootButtonUp();
                    }
                }
            }
            if (Time.time - dpTiming1 > dpCD1 && dpRestore && Time.time - dpTiming2 > dpCD2)//脱离战场&&防御值需要恢复&&恢复CD完成
            {
                dpTiming2 = Time.time;
                playerDP.realValue++;
                if (playerDP.realValue >= playerDP.maxValue)
                {
                    playerDP.realValue = playerDP.maxValue;
                    dpRestore = false;
                }
            }

        }

    }
    private void FixedUpdate()
    {
        if (!isDie)
        {
            myRigidbody.MovePosition(myRigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
    public void AddCoin(int data = 1)
    {
        coin += data;
        GameManager.instance.UpdateCoinNumber();
    }
    public void AddMagic(int data = 1)
    {
        if (playerMP.realValue < playerMP.maxValue)
        {
            playerMP.realValue += data;
        }
    }
    void GetWeapon()
    {
        if (weaponInFloor != null)//地上有枪就换枪
        {
            if (weapon != null)
            {
                myWeapon.transform.SetParent(GameManager.instance.weaponRecycle);
                weapon.PickDown();
            }
            myWeapon = weaponInFloor;
            weapon = myWeapon.GetComponent<Weapon>();
            weapon.Pickup();
            myWeapon.transform.SetParent(transform);
            myWeapon.transform.localPosition = new Vector3(0, 0, 0);
            myWeapon.transform.localRotation = Quaternion.identity;
            //注册
            weapon.Initialization(gameObject.tag, gameObject.layer);
        }
    }
    void SetLookAt()
    {
        if (myRigidbody.position.x > target.x)
        {
            myPlayer.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (myRigidbody.position.x < target.x)
        {
            myPlayer.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (weapon != null)
        {
            weapon.UpdateLookAt(target);
        }
    }

    /// <summary>
    /// 圆形检测
    /// </summary>
    public void Ray2DForCircle()
    {
        weaponInFloor = null;
        GameManager.instance.CloseTips();
        //int layer = 1 << LayerMask.NameToLayer("RayLayer");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, 0.5f, 0), 0.9f);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].CompareTag("Weapon"))
                {
                    weaponInFloor = cols[i].gameObject;
                }
                if (cols[i].GetComponent<GetTipsInfo>() != null)
                {
                    GameManager.instance.UpdateTipsInfo(cols[i].GetComponent<GetTipsInfo>().GetTipsInfo());
                }
            }
        }
    }

    public void BeAttack(float data)
    {
        dpRestore = true;
        dpTiming1 = Time.time;
        if (playerDP.realValue == 0)
        {
            playerHP.realValue -= data;
        }
        else if (playerDP.realValue < data)
        {
            playerHP.realValue -= data - playerMP.realValue;
            playerDP.realValue = 0;
        }
        else
        {
            playerDP.realValue -= data;
        }
        if (playerHP.realValue <= 0)
        {
            playerAnima.SetBool("die", true);
            isDie = true;
            GameManager.instance.GameOver();
            GetComponent<Collider2D>().enabled = false;
            GameManager.instance.ClearPlayerInfo();
            Destroy(myPlayer);
            Destroy(myWeapon);
        }
        else
        {
            playerAnima.Play("BeAttack");
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            GameManager.instance.ShowAttack(data, Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0)));
        }
    }
}
