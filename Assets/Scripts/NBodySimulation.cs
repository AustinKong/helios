using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    public static CelestialBody[] allBodies;

    private void Awake() => FindCelestialBodies();

    public static void FindCelestialBodies() => allBodies = FindObjectsOfType<CelestialBody>();

    private void FixedUpdate()
    {
        try
        {
            foreach (CelestialBody body in allBodies)
            {
                body.GravitationalUpdate(allBodies);
            }

            foreach (CelestialBody body in allBodies)
            {
                body.UpdatePosition();
            }
        }
        catch
        {
            FindCelestialBodies();
        }
    }
}
