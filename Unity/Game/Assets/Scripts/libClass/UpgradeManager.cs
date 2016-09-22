using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public abstract class UpgradeManager //: IUpgrade
{
    public UpgradeManager()
    {

    }
    public int id { get; private set; }
    public float value { get; private set; }
    public bool isOne { get; private set; }
    public byte count { get; private set; }
    public PlayerClassEnum playerClass{ get; private set; }
    public abstract void Upgrade(AbstractSkill skil);
}

