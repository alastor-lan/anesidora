﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class EnLighten1 : MonoBehaviour
{
    private Animator animator;
    private bool canLighten;
    public GameObject  torch2;
    public GameObject  torch3;
    public GameObject torch1;
    // public bool isLight1,isLight2,isLight3;
    // public GameObject torch1,torch2,torch3;
    private bool canChat=false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        canLighten = true;
        Debug.Log("碰撞发生");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        canLighten = false;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            Light();
        }
    }
    void Light()
    {
        if (canLighten)
        {
            if (!torch2.GetComponent<Animator>().GetBool("isLight2")  && !torch3.GetComponent<Animator>().GetBool("isLight3"))
            {
                canChat = false;
                torch1.GetComponent<Animator>().SetBool("isLight1", true);
            }
            else if(torch2.GetComponent<Animator>().GetBool("isLight2") || torch3.GetComponent<Animator>().GetBool("isLight3"))
            {
                torch1.GetComponent<Animator>().SetBool("isLight1", true);
                torch2.GetComponent<Animator>().SetBool("isLight2",false) ; torch3.GetComponent<Animator>().SetBool("isLight3",false);
                canChat = true;
                Say();
            }
        
        }
       
    }
    void Say()
    {
        if (canChat)
        {
            //对话
            Flowchart flowChart = GameObject.Find("CantOpened").GetComponent<Flowchart>();
            if (flowChart.HasBlock("CantOpen"))
            {
                //执行对话
                flowChart.ExecuteBlock("CantOpen");
            }
        }
    }
}
