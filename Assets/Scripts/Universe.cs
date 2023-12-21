using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public const float timeStep = 0.02f;
    public const float gravitationalConstant = 0.6f; //6.67e-11f
    private void Awake()
    {
        Application.targetFrameRate = -1;
        Time.fixedDeltaTime = timeStep;
    }
}
