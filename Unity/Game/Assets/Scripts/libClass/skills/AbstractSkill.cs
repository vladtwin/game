using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;


[Serializable]
public  abstract class AbstractSkill  : NetworkBehaviour 
{
    public SkillProperties properties { get; protected set; }
    public bool isUse;
    public List<AbstractSkillUse> next { get; protected set; }
    public int id { get; protected set; }
    public int playerId { get; protected set; }

  
    [NonSerialized]
    public GameObject effect;
    [NonSerialized]
    public GameObject dagameEffect;
    [NonSerialized]
    public GameObject destroyDagame;
    [NonSerialized]
    public List<GameObject> players;
    public bool destroyOnGamage { get; protected set; }
    
    public Vector3 target { get;  set; }
    public Vector3 move { get; set; }

    public delegate void onGamageCount(int count);
    public delegate void onKillCount(int count);
   
    public abstract void Destroy();
    public abstract void StartUse(SkillProperties properties, Vector3 target, Vector3 start, int playerId);
   
  
}

