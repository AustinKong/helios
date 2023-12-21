using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hotkeys : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R) && !CelestialBodyCreationManager.instance.customizeCelestialBodyPanel.activeInHierarchy)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (!CelestialBodyCreationManager.instance.customizeCelestialBodyPanel.activeInHierarchy)
        {
            CelestialBodyCreationManager.instance.eccentricity += Input.GetAxisRaw("Vertical") * Time.deltaTime;
            CelestialBodyCreationManager.instance.eccentricity = Mathf.Clamp(CelestialBodyCreationManager.instance.eccentricity, 0, 0.99f);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !CelestialBodyCreationManager.instance.customizeCelestialBodyPanel.activeInHierarchy)
        {
            PlanetDataPanel.instance.CloseDataPanel();
            CelestialBody[] cog = FindObjectsOfType<CelestialBody>();
            if (cog.Length == 0) return;
            PlanetDataPanel.instance.selectedCelestialBody = cog[Random.Range(0, cog.Length)];
            PlanetDataPanel.instance.OpenDataPanel();
        }

        if (Input.GetKeyDown(KeyCode.Delete) && !CelestialBodyCreationManager.instance.customizeCelestialBodyPanel.activeInHierarchy)
        {
            Destroy(PlanetDataPanel.instance.selectedCelestialBody.infoText.transform.parent.gameObject);
            Destroy(PlanetDataPanel.instance.selectedCelestialBody.gameObject);
            PlanetDataPanel.instance.CloseDataPanel();
            NBodySimulation.FindCelestialBodies();
        }
    }
}
