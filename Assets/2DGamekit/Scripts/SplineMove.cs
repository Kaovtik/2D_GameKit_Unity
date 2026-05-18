using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMove : MonoBehaviour
{
    public Spline spline;
    public Transform followObj;

    private Transform thisTrans;

    // Start is called before the first frame update
    void Start()
    {
        thisTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        thisTrans.position = spline.WhereOnSpline(followObj.position); 
    }
}
