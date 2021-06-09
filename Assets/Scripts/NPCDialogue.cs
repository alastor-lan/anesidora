﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NPCDialogue : MonoBehaviour
{
    public string ChatName;//这个名字是FlowChart中，块的名字，需要把这个变量设定为flowchart中块的名字
    // Start is called before the first frame update
    private bool canChat = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        canChat = true;
        Debug.Log("碰撞发生");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        canChat = false;
        Debug.Log(canChat);
    }
    private void Update()
    {
        
          if (Input.GetKeyDown(KeyCode.F))
        {
            Say();
        }
        
       
    }
    private void OnMouseDown()
    {
        Say();
    }
    void Say()
    {
        if (canChat)
        {
            //对话
            Flowchart flowChart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowChart.HasBlock(ChatName))
            {
                //执行对话
                flowChart.ExecuteBlock(ChatName);
            }
        }
    }
}