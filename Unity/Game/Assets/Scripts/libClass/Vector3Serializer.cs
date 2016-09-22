﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct Vector3Serializer
{
    public float x;
    public float y;
    public float z;
    public void Fill(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    

    public Vector3 V3
    { get { return new Vector3(x, y, z); } }
}