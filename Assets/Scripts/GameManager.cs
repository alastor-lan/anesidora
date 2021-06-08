﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
interface BeAttack
{
    void BeAttack(float data);
}
interface GetTipsInfo
{
    string GetTipsInfo();
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Image HPImage;
    public Text HPText;
    public Image DPImage;
    public Text DPText;
    public Image MPImage;
    public Text MPText;
    public GameObject gameOverGO;
    public GameObject gamePauseGO;
    public Button restart;
    public Button continued;
    public Text coinText;

    public GameObject beAttackText;
    public LittleMap littleMap;
    public Transform weaponRecycle;
    public GameObject tips;
    public List<GameObject> troops;

    public GameObject coinPre;
    public GameObject mpPre;

    PlayerControl player;
    Room room;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        troops = new List<GameObject>();
        troops.Add(PlayerControl.instance.gameObject);
        //找到所有房间进行初始化
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Room");
        for (int i = 0; i < gos.Length; i++)
        {
            gos[i].GetComponent<Room>().Initialization();
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        UpdateCoinNumber();
    }
    void Update()
    {
        if (room != null)
        {
            room.UpdateRoomInfo();
        }

        UpdatePlayerInfo();

    }
    public void UpdateCoinNumber()
    {
        coinText.text = player.coin.ToString();
    }
    public void UpdatePlayerInfo()
    {
        HPImage.fillAmount = player.playerHP.realValue / player.playerHP.maxValue;
        HPText.text = player.playerHP.realValue + "/" + player.playerHP.maxValue;
        DPImage.fillAmount = player.playerDP.realValue / player.playerDP.maxValue;
        DPText.text = player.playerDP.realValue + "/" + player.playerDP.maxValue;
        MPImage.fillAmount = player.playerMP.realValue / player.playerMP.maxValue;
        MPText.text = player.playerMP.realValue + "/" + player.playerMP.maxValue;
    }
    public void GameOver()
    {
        gameOverGO.SetActive(true);
        restart.onClick.AddListener(() => { SceneManager.LoadScene("Chapter01"); });
    }
    public void Gamepause()
    {
        Time.timeScale = 0;
        gamePauseGO.SetActive(true);
        continued.onClick.AddListener(() => { Time.timeScale = 1; gamePauseGO.SetActive(false); });
    }
    public void UpdatePlayerRoom(Room room)
    {
        this.room = room;
        //更新地图显示
        littleMap.UpdatePlayerIndex(room.roomNum);
        if (!room.isExplored)
        {
            littleMap.UpdateCanExploreInMap(room);
            littleMap.UpdateRoomsExploredInMap(room.roomNum);
        }
    }
    public void UpdateTipsInfo(string s)
    {
        tips.SetActive(true);
        tips.GetComponent<Text>().text = s;
    }
    public void CloseTips()
    {
        tips.SetActive(false);
    }
    public void ShowAttack(float data, Vector3 pos)
    {
        GameObject textgo = Instantiate(beAttackText, pos, Quaternion.identity);
        textgo.transform.SetParent(GameObject.Find("Canvas").transform);
        textgo.GetComponent<Text>().text = "-" + data;
    }
}