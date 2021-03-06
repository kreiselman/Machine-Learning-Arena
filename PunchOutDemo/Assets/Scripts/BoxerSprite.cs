﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A representation of the boxer sprite
/// </summary>
public class BoxerSprite : MonoBehaviour
{
    Animator anim;

    private Boxer boxer;
    private BoxerAudio boxerAudio;

    //Const Values (you can't declare Vector3s const, so just pretend)
    protected Vector3 LDEFAULT = new Vector3(-1, 0, 0);             //Default position of the left glove
    protected Vector3 RDEFAULT = new Vector3(1, 0, 0);              //Default position of the right glove
    protected Vector3 PUNCH = new Vector3(0, 1.7f, 0);                 //Distance a glove moves forward during punch

    public float dodgeDistance = 3;

    Vector3 LDODGELOC;                     //Number of units to move when dodging left
    Vector3 RDODGELOC;                     //Number of units to move when dodging right
    Vector3 BDODGELOC = new Vector3(0, -2, 0);                      //Number of units to move when dodging back
    Vector3 BLOCKANGLE = new Vector3(0, 0, 45);                     //Angle to move arms inward when blocking

    protected int punchTime = 0;                                    //number of frames left in punch animation

    private Renderer leftGloveRenderer, rightGloveRenderer, bodyRenderer;

    private Color gloveColor;

    private Vector3 DEFAULT = new Vector3(0, 0, 0);                  //Default position of the boxer

    float damageTime = 0;
    float maxDamageTime = .25f;

    enum Telegraphing
    {
        None,
        PunchLeft,
        PunchRight,
        DodgeLeft,
        DodgeRight
    }
    private Telegraphing tel = Telegraphing.None;
    private float intensity = 0;

    // Start is called before the first frame update
    void Start()
    {
        boxer = GetComponent<Boxer>();
        DEFAULT = this.transform.Find("Sprite").localPosition;
        boxer.dodgeAction.animationStart.AddListener(StartDodgeAnimation);
        boxer.dodgeAction.animationEnd.AddListener(StopDodgeAnimation);
        boxer.punchAction.animationStart.AddListener(StartPunchAnimation);
        boxer.punchAction.animationEnd.AddListener(StopPunchAnimation);
        boxer.punchAction.action.AddListener(PunchAction);

        boxer.hitEvent.AddListener(ShowDamage);

        GameObject lg = this.transform.Find("Sprite").Find("LeftArm").gameObject;
        leftGloveRenderer = lg.GetComponent<Renderer>();

        GameObject rg = this.transform.Find("Sprite").Find("RightArm").gameObject;
        rightGloveRenderer = rg.GetComponent<Renderer>();

        bodyRenderer = this.transform.Find("Sprite").Find("Body").GetComponent<Renderer>();

        gloveColor = rightGloveRenderer.material.color;

        LDODGELOC = new Vector3(-dodgeDistance, 0, 0);
        RDODGELOC = new Vector3(dodgeDistance, 0, 0);

        anim = GetComponent<Animator>();
        boxerAudio = GetComponent<BoxerAudio>();
    }

    private void StartDodgeAnimation(Direction direction)
    {
        anim.ResetTrigger("DodgeEnd");
        if (boxerAudio != null)
        {
            boxerAudio.PlayDodge();
        }
        if (direction == Direction.LEFT)
        {
            //this.transform.Find("Sprite").localPosition = DEFAULT + LDODGELOC;
            anim.Play("DodgeLeft");
        } else
        {
            //this.transform.Find("Sprite").localPosition = DEFAULT + RDODGELOC;
            anim.Play("DodgeRight");
        }
    }

    private void StopDodgeAnimation(Direction direction)
    {
        anim.SetTrigger("DodgeEnd");
    }

    private void StartPunchAnimation(Direction side)
    {
        //if (!boxer.broadcastPunch) return;
        Color broadcastColor = Color.red;
        if (side == Direction.LEFT)
        {
            //leftGloveRenderer.material.color = broadcastColor;
            //tel = Telegraphing.PunchLeft;
            anim.Play("PunchLeft");
        } else
        {
            //rightGloveRenderer.material.color = broadcastColor;
            //tel = Telegraphing.PunchRight;
            anim.Play("PunchRight");
        }
    }

    private void PunchAction(Direction side)
    {
        tel = Telegraphing.None;
        intensity = 0;
        if (side == Direction.LEFT)
        {
            Transform t = this.transform.Find("Sprite").Find("LeftArm");
            //t.localPosition = LDEFAULT;
            leftGloveRenderer.material.color = gloveColor;
            //t.localPosition = t.localPosition + PUNCH;
            //anim.Play("PunchLeft");
        }
        else
        {
            Transform t = this.transform.Find("Sprite").Find("RightArm");
            //t.localPosition = RDEFAULT;
            rightGloveRenderer.material.color = gloveColor;
           //anim.Play("PunchRight");
            //t.localPosition = t.localPosition + PUNCH;
        }
    }

    private void StopPunchAnimation(Direction side)
    {
        Transform left = this.transform.Find("Sprite").Find("LeftArm");
        //left.localPosition = LDEFAULT;

        Transform right = this.transform.Find("Sprite").Find("RightArm");
        //right.localPosition = RDEFAULT;

        leftGloveRenderer.material.color = gloveColor;
        rightGloveRenderer.material.color = gloveColor;
        anim.StopPlayback();
    }

    public void ShowDamage()
    {
        damageTime = maxDamageTime;
        bodyRenderer.material.color = Color.red;
        if (boxerAudio != null)
        {
            boxerAudio.PlayHit();
        }
        //print("Invoked");
    }

    // Update is called once per frame
    void Update()
    {
        if (damageTime > 0)
        {
            damageTime -= Time.deltaTime;

            if (damageTime <= 0)
                bodyRenderer.material.color = gloveColor;
        }
    }

    void ResetAnim()
    {
        anim.ResetTrigger("DodgeEnd");
    }
}
