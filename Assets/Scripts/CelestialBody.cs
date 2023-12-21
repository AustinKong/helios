using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class CelestialBody : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public Vector2 currentVelocity;

    public string customName;
    public float mass; // in 10^24kg
    public float diameter;
    public float density;

    public TMP_Text infoText;

    private void Awake() => rigidBody = GetComponent<Rigidbody2D>();

    public void AddVelocity(Vector2 velocity)
    {
        currentVelocity += velocity;
    }

    public void GravitationalUpdate(CelestialBody[] allBodies)
    {
        foreach (CelestialBody otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDist = (otherBody.rigidBody.position - rigidBody.position).sqrMagnitude;
                Vector2 forceDir = (otherBody.rigidBody.position - rigidBody.position).normalized;
                Vector2 force = forceDir * Universe.gravitationalConstant * mass * otherBody.mass / sqrDist;
                Vector2 acceleration = force / mass;
                currentVelocity += acceleration * Universe.timeStep;
            }
        }
    }

    public void UpdatePosition()
    {
        rigidBody.position += currentVelocity * Universe.timeStep;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<CelestialBody>().mass == mass)
        {
            mass += Random.Range(-0.00001f, 1f);
        }

        if (collision.gameObject.GetComponent<CelestialBody>().mass > mass)
        {
            //let the one with higher mass deal with this
            NBodySimulation.allBodies = NBodySimulation.allBodies.Where(val => val != this).ToArray();
            return;
        }
        else
        {
            diameter += collision.gameObject.GetComponent<CelestialBody>().diameter/3f;
            mass = BodyMath.CalculateMass(diameter, density);

            TMP_Text[] screw = infoText.transform.parent.GetComponentsInChildren<TMP_Text>(true);
            screw[2].text = diameter.ToString() + " km";
            screw[4].text = mass.ToString() + " kg";
            transform.localScale = Vector3.one * diameter;

            Destroy(collision.gameObject.GetComponent<CelestialBody>().infoText.transform.parent.gameObject);
            Destroy(collision.gameObject);

        }
    }

    public bool displayArrows = false;

    private void Update()
    {
        infoText.text = (currentVelocity.magnitude<0.1f?"0":currentVelocity.magnitude.ToString()) +" m/s";
        if (displayArrows)
        {
            ArrowGenerator.instance.DrawArrow(rigidBody.position, rigidBody.position + currentVelocity, Color.green);
            ArrowGenerator.instance.DrawArrow(rigidBody.position, rigidBody.position + new Vector2(currentVelocity.x, 0), Color.red);
            ArrowGenerator.instance.DrawArrow(rigidBody.position, rigidBody.position + new Vector2(0, currentVelocity.y), Color.blue);
        }
    }
}
