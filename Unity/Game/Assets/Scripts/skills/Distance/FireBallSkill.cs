using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FireBallSkill : AbstractSkillUse
{
    public override void StartUse(SkillProperties properties, Vector3 target, Vector3 start)
    {
        this.properties = properties;
        this.target = target;
        this.isUse = true;

       // target = transform.position + (target - transform.position).normalized * properties.ragne;
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
                transform.position += (target - transform.position).normalized * properties.speed * Time.deltaTime;
                if (Vector3.Distance(transform.position, target) < 1f)
                    Destroy(gameObject);
            }
        }
	}
    
}
