using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;







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

