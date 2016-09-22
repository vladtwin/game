using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


[Serializable]
public class SkillContainer
{
    public int skillId;
    public GameObject skillObject;
    public SkillProperties spoperties;
    public SkillContainer() { }
    public SkillContainer(int id, GameObject obj, SkillProperties sProperties)
    {
        skillId = id;
        skillObject = obj;
        spoperties = sProperties;
    }
}
