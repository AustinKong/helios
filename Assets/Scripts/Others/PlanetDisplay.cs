using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDisplay : MonoBehaviour
{
    Camera planetDisplayCamera;
    private void Awake() => planetDisplayCamera = gameObject.GetComponent<Camera>();

    /*
    private void Update()
    {
        if(PlanetDataPanel.instance.selectedPlanet != null)
        {
            planetDisplayCamera.transform.position = PlanetDataPanel.instance.selectedPlanet.transform.position + new Vector3(0, 0, -10);
            planetDisplayCamera.orthographicSize = PlanetDataPanel.instance.selectedPlanet.diameter;
        }
    }
    */
}
