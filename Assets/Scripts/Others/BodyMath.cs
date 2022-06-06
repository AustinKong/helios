using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMath : MonoBehaviour
{
    public static float CalculateMass(float _diameter, float _density) => 4.19f * Mathf.Pow(_diameter, 3) * _density;

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
