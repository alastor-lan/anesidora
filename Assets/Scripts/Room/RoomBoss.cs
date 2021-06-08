using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBoss : Room
{
    public Monster Boss;
    public GameObject endDoor;
    public GameObject healthBar;
    public override void Initialization()
    {
        base.Initialization();
        Boss.Initialization(this);
    }
    public override void MonsterDie(Monster monster)
    {
        base.MonsterDie(monster);
        endDoor.SetActive(true);
        isExplored = true;
    }
    public override void PlayerEnter()
    {
        base.PlayerEnter();
        if (!isExplored)
        {
            CloseDoor();
            Boss.isStart = true;
            healthBar.SetActive(true);
        }
    }
}
