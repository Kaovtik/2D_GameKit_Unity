using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DistanceCalculator : MonoBehaviour
{
    //private Transform playerTransform;
    //private float distance;

    private bool calculateDistance;

    [SerializeField] private StudioEventEmitter distanceEmitter;
    private StudioEventEmitter receivingEmitter;
    [SerializeField] private string sendingParameterName, receivingEmitterTag, receivingParameterName;
    
    // Start is called before the first frame update
    void Start()
    {
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        receivingEmitter = GameObject.FindGameObjectWithTag(receivingEmitterTag).GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (calculateDistance)
        {
            //distance = Vector3.Distance(gameObject.transform.position, playerTransform.position);
            //Debug.Log("distance is: " + distance);
            distanceEmitter.EventInstance.getParameterByName(sendingParameterName, out float value, out float finalValue);
            
            Debug.Log("value is: " + finalValue);
                    
            receivingEmitter.SetParameter(receivingParameterName, finalValue);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            calculateDistance = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            calculateDistance = false;  
        }
    }
}
