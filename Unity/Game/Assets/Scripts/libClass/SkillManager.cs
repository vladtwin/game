using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Networking;
using System.ComponentModel;





public  class SkillManager  {   

    public static SkillManager singleton=new SkillManager();
    List<SkillContainer> skills = new List<SkillContainer>();
    static SkillManager()
    {
        singleton.skills = new List<SkillContainer>();
        singleton.skills.Add(
            new SkillContainer(1,
            Resources.Load<GameObject>("Prefabs/skills/Distance/skillFireBall")
            , new SkillProperties(1, 2,10))
            );

     //   singleton.skills.Add(new FireBallSkill());
        //add new skill
    }
    public SkillManager()
    {

    }
    public SkillManager(bool createAllSkills)
    {
           

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
    
}
