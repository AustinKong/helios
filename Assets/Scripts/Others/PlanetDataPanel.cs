using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlanetDataPanel : MonoBehaviour
{
    public static PlanetDataPanel instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Others")]
    public CelestialBody selectedCelestialBody;
    public Camera mainCamera;

    private void Update()
    {
        CheckPlanetClick();
    }

    private void CheckPlanetClick()
    {
        if (Input.GetMouseButtonDown(0) && !CelestialBodyCreationManager.instance.customizeCelestialBodyPanel.activeInHierarchy && !CelestialBodyCreationManager.instance.placementDisplayEnabled)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, mousePos2D);

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (hit)
                {
                    CelestialBody cog;
                    if (hit.collider.gameObject.TryGetComponent<CelestialBody>(out cog))
                    {
                        CloseDataPanel();
                        selectedCelestialBody = cog;
                        OpenDataPanel();
                    }
                }
                else
                {
                    CloseDataPanel();
                }
            }
            
        }
    }

    public void OpenDataPanel()
    {
        try
        {
            TMP_Text[] screw = selectedCelestialBody.infoText.transform.parent.GetComponentsInChildren<TMP_Text>(true);
            screw[2].gameObject.SetActive(true);
            screw[3].gameObject.SetActive(true);
            screw[4].gameObject.SetActive(true);
            selectedCelestialBody.displayArrows = true;
        }
        catch
        {
            CloseDataPanel();
        }
    }

    public void CloseDataPanel()
    {
        if (selectedCelestialBody == null) return;
        TMP_Text[] screw = selectedCelestialBody.infoText.transform.parent.GetComponentsInChildren<TMP_Text>(true);
        screw[2].gameObject.SetActive(false);
        screw[3].gameObject.SetActive(false);
        screw[4].gameObject.SetActive(false);
        selectedCelestialBody.displayArrows = false;
        selectedCelestialBody = null;
    }
}
