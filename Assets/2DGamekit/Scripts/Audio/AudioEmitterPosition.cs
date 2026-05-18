using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitterPosition : MonoBehaviour
{

    public string tagName;
    
    void Update()
    {
        if (GameObject.FindGameObjectWithTag(tagName) == null)
        {
            return;
        }
        else
        {
            transform.position = GameObject.FindGameObjectWithTag(tagName).GetComponent<Transform>().position;
        }
        
    }
}
