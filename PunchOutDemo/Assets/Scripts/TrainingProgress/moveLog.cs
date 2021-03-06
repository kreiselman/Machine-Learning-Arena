﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLog : MonoBehaviour
{
    string myLog;
    Queue myLogQueue = new Queue();
    Queue myPics = new Queue();
    int count = 0;
    Texture2D LP, LD, RP, RD;
    [SerializeField]
    public GameObject LeftDodge;
    public GameObject LeftPunch;
    public GameObject RightDodge;
    public GameObject RightPunch;
    //Vector3 center;


    public Boxer boxer;
    // Start is called before the first frame update
    void Start()
    {
        boxer = GetComponent<Boxer>();
        boxer.punchAction.animationStart.AddListener(logPunch);
        boxer.dodgeAction.animationStart.AddListener(logDodge);
        LP = Resources.Load("Key Logs/Left Punch") as Texture2D;
        LD = Resources.Load("Key Logs/Left Dodge") as Texture2D;
        RP = Resources.Load("Key Logs/Right Punch") as Texture2D;
        RD = Resources.Load("Key Logs/Right Dodge") as Texture2D;
        //center = Camera.main.WorldToScreenPoint(new Vector3(0, 0, -2));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void logPunch(Direction side)
    {
        if (side == Direction.LEFT)
        {
            //HandleLog("Left Punch");
            //  HandlePic(LP);
            Instantiate(LeftPunch, new Vector3(-9.5f, -1.5f, -2), Quaternion.identity);

        }
        else {
            //  HandleLog("Right Punch");
            //  HandlePic(RP);
            Instantiate(RightPunch, new Vector3(-9.5f, -1.5f, -2), Quaternion.identity);
        }
    }

    void logDodge(Direction side)
    {
        if (side == Direction.LEFT)
        {
            // HandleLog("Left Dodge");
           // HandlePic(LD);
            //Instantiate(myPrefab, new Vector3(0, 0, -2), Quaternion.identity);
            Instantiate(LeftDodge, new Vector3(-9.5f, -1.5f, -2), Quaternion.identity);
            //Debug.Log("created l Dodge at " + center.x + ", " + center.y);

        }
        else {
            //  HandleLog("Right Dodge");
           // HandlePic(RD);
            Instantiate(RightDodge, new Vector3(-9.5f, -1.5f, -2), Quaternion.identity);
        }
    }






    void HandleLog(string logString)
    {
        myLog = logString;
        string newString = "\n\n" + myLog;
        myLogQueue.Enqueue(newString);
        count++;
        if (count >= 5)
        {
            myLogQueue.Dequeue();
        }

        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }

    void HandlePic(Texture tex)
    {

        myPics.Enqueue(tex);
        count++;
        if (count >= 10)
        {
            myPics.Dequeue();
        }



    }
/*
    void OnGUI()
    {
        //var center = Camera.main.WorldToScreenPoint(new Vector3(8, -5, 0));
        GUILayout.BeginArea(new Rect(center.x, center.y, 100, 1200));
            
        // GUILayout.Label(myLog);
        foreach (Texture2D tx in myPics)
        {
            GUILayout.Label(tx);
        }

        GUILayout.EndArea();

    }

    */

}
