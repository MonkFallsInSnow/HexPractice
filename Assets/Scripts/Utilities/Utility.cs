﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float Map (float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}

public class Vector3CoordComparer : IEqualityComparer<Vector3> 
{
     public bool Equals(Vector3 a, Vector3 b) {
        if (Mathf.Abs(a.x - b.x) > 0.001) return false;
        if (Mathf.Abs(a.y - b.y) > 0.001) return false;
        if (Mathf.Abs(a.z - b.z) > 0.001) return false;
 
         return true; //indeed, very close
     }
 
     public int GetHashCode(Vector3 obj) {
         //a cruder than default comparison, allows to compare very close-vector3's into same hash-code.
         return Math.Round(obj.x, 3).GetHashCode() 
              ^ Math.Round(obj.y, 3).GetHashCode() << 2 
              ^ Math.Round(obj.z, 3).GetHashCode() >> 2;
     }
}

