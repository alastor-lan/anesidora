using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    public string chapterName;
    public bool isRight;
    [SerializeField] private string newchapterPassword;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRight)
        {
            if (collision.transform.tag =="Player")
            {
                PlayerControl.instance.chapterPassword = newchapterPassword;
                //SceneManager.LoadScene(chapterName);
                FindObjectOfType<chapterFader>().FadeTo(chapterName);
            }
        }
        else
         if (collision.transform.tag == "Player")
        {
            GameManager.instance.GameOver();
        }
    }
}
