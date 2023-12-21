using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public enum CelestialBodyType
{
    Star,
    Planet,
    Moon,
    Satelite,
    Comet
}

public class CelestialBodyData
{
    public float diameter;
    public float density;
    public float mass;
    public string customName;
    public Color color;
    public CelestialBodyType type;
}

public class CelestialBodyCreationManager : MonoBehaviour
{
    public static CelestialBodyCreationManager instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Celestial Body Prefabs")]
    public GameObject starPrefab;
    public GameObject planetPrefab;
    public GameObject moonPrefab;
    public GameObject satelitePrefab;
    public GameObject cometPrefab;

    [Header("Others")]
    public GameObject infoTextPrefab;
    public GameObject pickCelestialBodyPanel;
    public GameObject customizeCelestialBodyPanel;

    [Header("Data References")]
    public TMP_InputField _name;
    public TMP_InputField _density;
    public TMP_InputField _diameter;
    public TMP_InputField _mass;
    public CUIColorPicker _color;

    public TMP_Text _eccentricity;

    private CelestialBodyData bufferCelestialBody;

    public float eccentricity = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !pickCelestialBodyPanel.activeInHierarchy && !customizeCelestialBodyPanel.activeInHierarchy)
        {
            pickCelestialBodyPanel.SetActive(true);
        }

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (placementDisplayEnabled)
        {
            //need to display path if more than 1
            if (NBodySimulation.allBodies.Length > 0)
            {
                float angle = Mathf.Deg2Rad * Vector3.Angle((Vector3)mousePos - closestBody.transform.position, Vector3.right);
                if (mousePos.y < closestBody.transform.position.y) angle *= -1;
                distanceToClosestBody = Vector2.Distance(closestBody.transform.position, mousePos);
                DisplayBodyPath(closestBody.transform.position, distanceToClosestBody, angle);
            }

            DisplayPlanetPreview(mousePos, bufferCelestialBody.diameter);
        }
        else line.positionCount = 0;

        if (Input.GetMouseButtonDown(0) && placementDisplayEnabled)
        {
            Vector2 delta = Vector2.zero;
            if (NBodySimulation.allBodies.Length > 0)
            {
                float a = distanceToClosestBody / (1 + eccentricity);

                float velocity = Mathf.Sqrt(Universe.gravitationalConstant * closestBody.mass * (2 / distanceToClosestBody - 1 / a));
                delta = Vector2.Perpendicular(mousePos - (Vector2)closestBody.transform.position).normalized * velocity + closestBody.currentVelocity;
            }
            GenerateCelestialBody(mousePos, delta);
            NBodySimulation.FindCelestialBodies();
            placementDisplayEnabled = false;
            preview.gameObject.SetActive(false);
        }

        _eccentricity.text = "Eccentricity: " + eccentricity.ToString();
    }

    public void PickCelestialBodyType(string type)
    {
        bufferCelestialBody = new CelestialBodyData();

        switch (type)
        {
            case "Star":
                DisplayCustomizeCelestialBodyPanel(25, 1f, "Star", Color.yellow, CelestialBodyType.Star);
                break;
            case "Planet":
                DisplayCustomizeCelestialBodyPanel(3, 1.5f, "Planet", Color.blue, CelestialBodyType.Planet);
                break;
            case "Moon":
                DisplayCustomizeCelestialBodyPanel(0.5f, 0.5f, "Moon", Color.gray, CelestialBodyType.Moon);
                break;
            case "Satelite":
                DisplayCustomizeCelestialBodyPanel(0.1f, 1f, "Satelite", Color.white, CelestialBodyType.Satelite);
                break;
            case "Comet":
                DisplayCustomizeCelestialBodyPanel(0.1f, 0.5f, "Comet", Color.white, CelestialBodyType.Comet);
                break;
        }

        pickCelestialBodyPanel.SetActive(false);
    }

    //purely drawing onto the ui
    private void DisplayCustomizeCelestialBodyPanel(float diameter, float density, string customName, Color customColor, CelestialBodyType type)
    {
        customizeCelestialBodyPanel.SetActive(true);
        _name.text = customName;
        _density.text = density.ToString();
        _diameter.text = diameter.ToString();
        _mass.text = BodyMath.CalculateMass(diameter, density).ToString();
        _color.Color = customColor==Color.blue?Random.ColorHSV():customColor;
        bufferCelestialBody.type = type;
    }

    public void UpdatedDiameterOrDensity()
    {
        float cog, bolt;
        if (float.TryParse(_diameter.text, out cog) && float.TryParse(_density.text, out bolt))
        {
            _mass.text = BodyMath.CalculateMass(cog, bolt).ToString();
        }

    }

    //--------------------------------------------------DRAWING THE PLACEMENT INDICATORS--------------------------------

    public Camera mainCamera;
    public LineRenderer line;
    public Transform preview;
    private int vertices = 24;

    private Vector2 mousePos;
    public bool placementDisplayEnabled = false;

    //CURRENT SYSTEM
    //Using intial mouse click position as reference for target orbit star
    //Player can click on star they wish to orbit and drag outwards

    private CelestialBody closestBody;
    private float distanceToClosestBody;

    public void EnableDisplay()
    {
        float cog;
        float bolt;
        if (!(float.TryParse(_diameter.text, out cog) && float.TryParse(_density.text, out bolt)))
        {
            return;
        }
        bufferCelestialBody.customName = _name.text;
        bufferCelestialBody.diameter = cog;
        bufferCelestialBody.density = bolt;
        bufferCelestialBody.mass = BodyMath.CalculateMass(bufferCelestialBody.diameter, bufferCelestialBody.density);
        bufferCelestialBody.color = _color.Color;

        if (NBodySimulation.allBodies.Length > 0)
        {
            if (PlanetDataPanel.instance.selectedCelestialBody != null)
            {
                closestBody = PlanetDataPanel.instance.selectedCelestialBody;
            }
            else
            {
                closestBody = GetClosestBody(NBodySimulation.allBodies, mousePos);
            }

        }
        placementDisplayEnabled = true;
        customizeCelestialBodyPanel.SetActive(false);
        preview.gameObject.SetActive(false);
        preview.GetComponent<SpriteRenderer>().color = bufferCelestialBody.color;
    }

    //-------------------------------------SKELETAL FUNCTIONS TO DRAWING DISPLAY---------------------

    private void DisplayBodyPath(Vector2 other, float distance, float angle)
    {

        List<Vector3> lineVertices = new List<Vector3>();
        for (int i = 0; i < vertices; i++)
        {
            float a, b, c;
            if (eccentricity == 0)
            {
                a = b = distance;
                c = 0;
            }
            else
            {
                a = distance / (1 + eccentricity);
                c = distance - a;
                b = Mathf.Sqrt(a * a - c * c);
            }

            float theta = Mathf.Deg2Rad * i * 360 / vertices;
            float phi = angle;

            float x = a * Mathf.Cos(theta);
            float y = b * Mathf.Sin(theta);

            Vector2 vertex = BodyMath.RotatePointAroundPivot(new Vector2(x, y), Vector2.zero, new Vector3(0, 0, phi * Mathf.Rad2Deg));
            vertex += other;
            vertex += new Vector2(c * Mathf.Cos(phi), c * Mathf.Sin(phi));
            lineVertices.Add(vertex);
        }

        line.positionCount = vertices;
        line.SetPositions(lineVertices.ToArray());
    }

    private void DisplayPlanetPreview(Vector2 position, float diameter)
    {
        preview.gameObject.SetActive(true);
        preview.transform.position = position;
        preview.localScale = Vector3.one * diameter;
    }

    private CelestialBody GetClosestBody(CelestialBody[] bodies, Vector3 pos)
    {
        CelestialBody bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (CelestialBody potentialTarget in bodies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - pos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    //---------------------------------------------------------INSTANTIATE PLANET-------------------------

    public GameObject GenerateCelestialBody(Vector2 position, Vector2 velocity)
    {
        GameObject cog;
        switch (bufferCelestialBody.type)
        {
            case CelestialBodyType.Star:
                cog = Instantiate(starPrefab, position, Quaternion.identity);
                break;
            case CelestialBodyType.Planet:
                cog = Instantiate(planetPrefab, position, Quaternion.identity);
                break;
            case CelestialBodyType.Moon:
                cog = Instantiate(moonPrefab, position, Quaternion.identity);
                break;
            case CelestialBodyType.Satelite:
                cog = Instantiate(satelitePrefab, position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
                break;
            case CelestialBodyType.Comet:
                cog = Instantiate(cometPrefab, position, Quaternion.identity);
                break;
            default:
                cog = Instantiate(starPrefab);
                break;
        }

        //instantiate info text
        GameObject rig = Instantiate(infoTextPrefab, position + new Vector2(1f, -1f) * bufferCelestialBody.diameter / 2f, Quaternion.identity);
        rig.GetComponent<ParentOnlyPosition>().parent = cog.transform;

        TMP_Text[] screw = rig.GetComponentsInChildren<TMP_Text>(true);

        screw[0].text = bufferCelestialBody.customName;
        screw[2].text = bufferCelestialBody.diameter.ToString() + " km";
        screw[3].text = bufferCelestialBody.density.ToString() + " kg/m3";
        screw[4].text = BodyMath.CalculateMass(bufferCelestialBody.diameter, bufferCelestialBody.density).ToString() + " kg";

        SpriteRenderer bolt = cog.GetComponentInChildren<SpriteRenderer>();
        CelestialBody nut = cog.GetComponent<CelestialBody>();

        bolt.color = bufferCelestialBody.color;

        cog.transform.localScale = new Vector3(1, 1, 1) * bufferCelestialBody.diameter;

        Gradient gradient = new Gradient();
        GradientColorKey[] colorKey = new GradientColorKey[1];
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[1];

        colorKey[0].color = bufferCelestialBody.color;
        colorKey[0].time = 0.0f;

        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;

        gradient.SetKeys(colorKey, alphaKey);

        nut.infoText = screw[1];
        nut.diameter = bufferCelestialBody.diameter;
        nut.density = bufferCelestialBody.density;
        nut.mass = BodyMath.CalculateMass(bufferCelestialBody.diameter, bufferCelestialBody.density);
        nut.customName = bufferCelestialBody.customName;
        nut.AddVelocity(velocity);
        nut.GetComponentInChildren<TrailRenderer>().colorGradient = gradient;
        nut.GetComponentInChildren<TrailRenderer>().widthMultiplier = bufferCelestialBody.diameter;
        NBodySimulation.FindCelestialBodies();

        return cog;
    }
}
