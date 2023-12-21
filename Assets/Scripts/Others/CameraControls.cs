using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public static CameraControls instance;
    private void Awake()
    {
        instance = this;
    }

    readonly float zoomSpeed = 100f;
    readonly float dragSpeed = 1f;

    public Camera[] cameras;

    private Vector3 dragOrigin;

    private float camSize = 120f;
    // Update is called once per frame
    void Update()
    {
        camSize -= Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
        if (camSize < 1f) camSize = 1f;
        foreach (Camera cam in cameras)
        {
            cam.orthographicSize = camSize;
        }

        if (PlanetDataPanel.instance.selectedCelestialBody !=null && Input.GetKeyDown(KeyCode.F) && !CelestialBodyCreationManager.instance.customizeCelestialBodyPanel.activeInHierarchy)
        {
            camSize = PlanetDataPanel.instance.selectedCelestialBody.diameter * 3f;
        }

        //below is dragging code
        if (Input.GetMouseButtonDown(2))
        {
            if(PlanetDataPanel.instance.selectedCelestialBody != null) PlanetDataPanel.instance.CloseDataPanel();

            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(2))
        {
            foreach (Camera cam in cameras)
            {
                Vector3 pos = cam.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

                cam.transform.Translate(move, Space.World);
                //noPostProcessingCamera.transform.Translate(move, Space.World);
            }
        }

        if (PlanetDataPanel.instance.selectedCelestialBody != null)
        {
            foreach (Camera cam in cameras)
            {
                cam.transform.position = PlanetDataPanel.instance.selectedCelestialBody.transform.position + new Vector3(0, 0, -10);
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && !CelestialBodyCreationManager.instance.customizeCelestialBodyPanel.activeInHierarchy)
        {
            cameras[2].gameObject.SetActive(!cameras[2].gameObject.activeInHierarchy);
        }
    }

}
