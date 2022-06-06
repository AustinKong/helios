using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentOnlyPosition : MonoBehaviour
{
    public Transform parent;
    private Vector3 relativePositioning;

    private void Start()
    {
        relativePositioning = transform.position-parent.position;
    }

    private void Update()
    {
        transform.position = parent.position + relativePositioning;
    }
}
