﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Networking;
using System.ComponentModel;





public  class SkillManager  {
    private bool isRegistred;
    public static SkillManager singleton=new SkillManager();
    List<SkillContainer> skills = new List<SkillContainer>();
    static SkillManager()
    {
        singleton.skills = new List<SkillContainer>();


        SkillProperties fireballProperties = new SkillProperties(1, 1, 3, 10, 10, 20, 1, 1, PlayerClassEnum.no, PlayerSkillEnum.none, null, null, 0, 0, 0,"Fireball","Create firemall and damage players...",1,1);
        GameObject fireballObject = Resources.Load<GameObject>("Prefabs/skills/Distance/skillFireBall");        
        singleton.skills.Add(new SkillContainer(1, fireballObject,  fireballProperties));

    }
    public  void Register(SkillContainer skill)
    {
        if(skills.Exists(p=>p.skillId == skill.skillId)==false)
        skills.Add(skill);
    } 
    public SkillContainer getSkill(int id)
    {
        return skills.FirstOrDefault(p => p.skillId == id);
    }
   
    public  ReadOnlyCollection<SkillContainer> getSkills()
    {
        return skills.AsReadOnly();
    }

    public ReadOnlyCollection<SkillContainer> getSkills(PlayerClassEnum _enum)
    {
        return skills.Where(p => p.spoperties.playerClass == _enum).ToList().AsReadOnly();
    }

    public bool Upgrade(int skillId)
    {     
        SkillContainer temp = getSkill(skillId);
        if(temp==null)
        {
            Debug.LogError("Error");
            return false;
        }
        SkillProperties tempP = temp.spoperties;
        if (tempP == null) return false;
        temp.spoperties= tempP.UpdateNextLevel();
        return true;
    }
    public void register(NetworkManager nm)//register all skill object in network (from NetworkMasterClient in Start)
    {
        if(!isRegistred)
        {
            foreach(SkillContainer container in skills)
            {
                nm.spawnPrefabs.Add(container.skillObject);
            }
            isRegistred = true;
        }
    }
    
}
