using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

[Serializable]
public class MainNetworkInfo : MonoBehaviour
{
    public string Name;
    public SkillManager skills;

    void Start () {
       Name = "vasa";
       skills = new SkillManager(false);
    }
	
	
	void Update () {
	
	}
}
