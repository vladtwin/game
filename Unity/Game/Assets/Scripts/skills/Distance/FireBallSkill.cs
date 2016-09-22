using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class FireBallSkill : AbstractSkillUse
{
    public override void StartUse(SkillProperties properties, Vector3 target, Vector3 start,int playerId)
    {
        base.StartUse(properties, target, start, playerId);
        target.y = transform.position.y;
        this.target = transform.position + (target - transform.position).normalized * properties.ragne;
        this.move = (target - transform.position).normalized * properties.speed;
    }

    public override void Use()
    {
        isUse = true;
    }

	void Update () {
        if (isServer)
        {
            if (isUse)
            {
                List<GameObject> hitPlayers = GetPlayers();
                if(hitPlayers.Count>0)
                {
                    foreach(GameObject go in hitPlayers)
                    {
                        go.GetComponent<Player>().doSkillUse(this);
                    }
                    Destroy();
                }
                transform.position += move * Time.deltaTime;
                if (Vector3.Distance(transform.position, target) < 1f)
                    Destroy();
            }
        }
	}
    
}
