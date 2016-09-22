using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public abstract class AbstractSkillUse : AbstractSkill
{

    public override void Destroy()
    {
        Destroy(gameObject);
    }
    public abstract void Use();
    //public abstract void StartUse(SkillProperties properties, Vector3 target, Vector3 start,int playerId);
    public override void StartUse(SkillProperties properties, Vector3 target, Vector3 start, int playerId)
    {
        this.properties = properties;
        this.target = target;
        this.playerId = playerId;
        this.transform.position = start;
        base.isUse = true;
    }
    public List<GameObject> GetPlayers(float radisu)
    {
        List<GameObject> findObjects = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radisu);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player" && hitColliders[i].gameObject.GetComponent<Player>() != null && hitColliders[i].GetComponent<Player>().playerId!=playerId)
                findObjects.Add(hitColliders[i].gameObject);
            i++;
        }

        return findObjects;
    }
    public List<GameObject> GetPlayers()
    {
        return GetPlayers(base.properties.ragne);
    }



}
