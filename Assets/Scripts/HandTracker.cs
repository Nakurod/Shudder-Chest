using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracker : MonoBehaviour
{

    [SerializeField] private Transform followObject;
    [SerializeField] private Vector3 offsetRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followObject.position;
        transform.rotation = followObject.rotation * Quaternion.Euler(offsetRotation);
    }
}
