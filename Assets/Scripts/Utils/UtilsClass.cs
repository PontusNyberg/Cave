using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsClass {

    public static Vector3 GetVectorFromAngle(float angle) {
        // angle = 0 -> 360
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
    }
}