using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;


[Serializable]
public class SkillContainer
{ 
    public int skillId;
    public GameObject skillObject;
    public SkillProperties spoperties;
    public SkillContainer() { }
    public SkillContainer(int id,GameObject obj, SkillProperties sProperties)
    {
        skillId = id;
        skillObject = obj;
        spoperties = sProperties;
    }
}

[Serializable]
public class SkillCreateInfo
{
    public int skillId;
    public byte skillLevel;
    public byte playerId;
    public List<UpgradeManager> upgrade;
    public Vector3Serializer playerPosition;
    public Vector3Serializer targetPosition;
}
  /*
public interface iSkillProperties
{
     int id { get; set; }                 //number of the skill
     float damage { get;  set; }          // start damage to player   -- start ( no upgrade )
     float coolDown { get; set; }         
     float force { get; set; }
     float speed { get; set; }
     float radius { get; set; }
     float ragne { get; set; }
     float castTime { get; set; }
     PlayerClassEnum playerClass { get; set; }
     PlayerSkillEnum playerSkillType { get; set; }
    List<UpgradeManager> update { get; set; }
    iSkillProperties UpdateLevel(iSkillProperties p, int level);
} */
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


[Serializable]
public  abstract class AbstractSkill  : NetworkBehaviour 
{
    protected SkillProperties properties;
    public bool isUse;
    private List<AbstractSkillUse> next;
    public int id { get; protected set; }
    public int playerId { get;  set; }
    public string _name { get; protected set; }
    public string description { get; protected set; }
    public float coolDown { get; protected set; }
    public float damage { get; protected set; }
    [NonSerialized]
    public GameObject effect;
    [NonSerialized]
    public GameObject dagameEffect;
    [NonSerialized]
    public GameObject destroyDagame;
    public bool destroyOnGamage { get; protected set; }
    public float force { get; protected set; }
    public float radius { get; protected set; }
    public float speed { get; protected set; }
    public int destroyTime { get; protected set; }
    
    public Vector3 target { get;  set; }
    public Vector3 move { get; set; }
    public List<UpgradeManager> upgradeList { get; protected set; }
    public byte maxUpgradeCount { get; protected set; }
    public PlayerClassEnum type { get; protected set; }
    public float castTime { get; private set; }

    public float nowCoolDown { get; set; }
    public float nowDamage { get; set; }
    public float nowRadius { get; set; }
    public float nowSpeed { get; set; }
    public float nowLevel { get; set; }

    public delegate void onGamageCount(int count);
    public delegate void onKillCount(int count);
   
    public abstract void Destroy();
   // public abstract object Clone();
    

    public bool AddUpgrade(UpgradeManager upgrade)
    {
        if (type == upgrade.playerClass) return false;
        if (upgradeList.Count <= maxUpgradeCount) return false;
        if (upgradeList.Exists(p=>p.id==upgrade.id) == true) return false;        
        upgradeList.Add(upgrade);
        upgrade.Upgrade(this);
        return true;
    }
    public List<AbstractSkillUse> getNextSkill()
    {
        return next;
    }
}
[Serializable]
public abstract class AbstractSkillUse : AbstractSkill
{
   
    public override void Destroy()
    {
        Destroy(gameObject);
    }
    public abstract void Use();
    public abstract void StartUse(SkillProperties properties, Vector3 target, Vector3 start);

    public List<GameObject> GetPlayers(float radisu)
    {
        List<GameObject> findObjects = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radisu);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player" && hitColliders[i].gameObject.GetComponent<Rigidbody>() != null)
                findObjects.Add(hitColliders[i].gameObject);
            i++;
        }

        return findObjects;
    }



}
public abstract class CreatorSkill:NetworkBehaviour
{
    public AbstractSkillUse Skill { set
        {
            value.Use();
        } }
    public CreatorSkill(AbstractSkillUse skill)
    {
        Skill = skill;
    }


}
