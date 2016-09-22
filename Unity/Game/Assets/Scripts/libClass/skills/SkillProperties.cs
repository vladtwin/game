using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public  class SkillProperties 
{
    public int id { get; protected set; }                 //number of the skill
    public float damage { get; protected set; }          // start damage to player   -- start ( no upgrade )
    public float coolDown { get; protected set; }
    public float force { get; protected set; }
    public float speed { get; protected set; }
    public float radius { get; protected set; }
    public float ragne { get; protected set; }
    public float castTime { get; protected set; }
    public PlayerClassEnum playerClass { get; protected set; }
    public PlayerSkillEnum playerSkillType { get; protected set; }
    public List<UpgradeManager> update { get; protected set; }
    public List<float> updateCoof; // список кооф улучшений для уровня
    public int nowLevel { get; protected set; }
    public int maxLevel { get; protected set; }
    public int cost;
    public SkillProperties() { }
    public SkillProperties(int id,float castTime,float speed)
    {
        this.id = id;
        this.castTime = castTime;
        this.speed = speed;
    }
    public SkillProperties UpdateLevel(SkillProperties p, int level)
    {
        SkillProperties temp=ObjectCopier.Clone<SkillProperties>(p);
        nowLevel++;
        if(nowLevel>maxLevel || updateCoof == null||  nowLevel>updateCoof.Count)
        {
            Debug.LogError("Error");
            return null;
        }
        float cof = temp.updateCoof[nowLevel];
        temp.radius = p.radius * cof;
        temp.force = p.force * cof;

        return temp;
    }
    public SkillProperties UpdateNextLevel()
    {
        return UpdateLevel(this, nowLevel+1);
    } 
}