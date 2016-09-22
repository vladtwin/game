using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;




public class SkilllUsage: SkillManager
{
    public class SkillWait
    {
        public int id;
        public float time;
        public SkillWait(int id,float time)
        {
            this.id = id;
            this.time = time;
        }
    }
    public bool isCast;
    public float castProcentage { get
        {
            if (castTime == 0)
                return 0;
            return nowCastTime / castTime;
        }
    }
    SkillWait wait;
    public float castTime { get; private set; }
    public float nowCastTime { get; private set; }
    Vector3 target,startPosition;
    SkillContainer skill;
    public SkilllUsage() : base(false)
    {
       // StartCoroutine(WaitCast()); 
    }           
    public void SkillUse(int id,Vector3 _target, Vector3 _startPosition,int playerId)
    {
        if (wait!=null && wait.id==id)
            return;
        skill = SkillManager.singleton.getSkill(id);
        wait = (new SkillWait(id, skill.spoperties.castTime));
        castTime = wait.time;
        nowCastTime = wait.time;
        isCast = true;
        target = _target;
        startPosition = _startPosition;
    }
    public void AbortCast()
    {
        wait = null;
        nowCastTime = 0;
        castTime = 0;
        target = Vector3.zero;
        startPosition = Vector3.zero;
        isCast = false;
        skill = null;
    }
    public void Tick(float dt)
    {
        if(wait!=null)
        {
            wait.time -= dt;
            nowCastTime = wait.time;
            if (wait.time<=0)
            {
                try
                {
                  //  Debug.Log("Cast: " + wait.id);
                    GameObject skillObject = GameObject.Instantiate(SkillManager.singleton.getSkill(wait.id).skillObject, startPosition, new Quaternion()) as GameObject;
                    NetworkServer.Spawn(skillObject);
                    skillObject.GetComponent<AbstractSkillUse>().StartUse(skill.spoperties,target,startPosition);
                }
                catch { }
                AbortCast();
            }           
        }
       /* while (true) {
           
            if (nowCast != null ) {
                if (nowCast.time > 0)
                {
                    if (!isCast)
                        waitSkill = null;                    
                }
                if (isCast)
                {
                    wait.Add(new SkillWait() { id = waitSkill.id, time = waitSkill.coolDown });
                    waitSkill = null;
                }
            }
            foreach (SkillWait w in wait)
                w.time -=Time.deltaTime;
            int removeId =
            wait.RemoveAll(p => p.time <= 0);
           
        }  */
    }



}

