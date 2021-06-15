using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    public string chapterName;
    public bool isRight;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRight)
        {
            if (collision.transform.tag =="Player")
            {
               SceneManager.LoadScene(chapterName);
             }
        }
        else
         if (collision.transform.tag == "Player")
        {
            // GameManager.instance.KillPlayer();
            PlayerControl.instance.playerDP.realValue=0;
            PlayerControl.instance.BeAttack(PlayerControl.instance.playerHP.realValue);
        }
    }
}
