using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SaveLocalParamValues : MonoBehaviour
{
    public static SaveLocalParamValues Instance;

    public List<Tuple<StudioEventEmitter, string, float>> savedParameterValues =
        new List<Tuple<StudioEventEmitter, string, float>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public float SaveParameterValue(StudioEventEmitter emitter, string parameter, float parameterValue)
    {
        Tuple<StudioEventEmitter, string, float> savedParam =
            new Tuple<StudioEventEmitter, string, float>(emitter, parameter, parameterValue);

        if (savedParameterValues.Count == 0)
        {
            savedParameterValues.Add(savedParam);
            return savedParam.Item3;
        }
        
        for (int i = 0; i < savedParameterValues.Count; i++)
        {
            if (savedParameterValues[i].Item1 == emitter && savedParameterValues[i].Item2 == parameter)
            {
                if (savedParameterValues[i].Item3 < parameterValue)
                {
                    savedParameterValues.Insert(i, savedParam);
                    return savedParam.Item3;
                }
                else
                {
                    return savedParameterValues[i].Item3;
                }
            }
        }
        savedParameterValues.Add(savedParam);
        return savedParam.Item3;
    }

    /*public float GetParameterValueFromListe(StudioEventEmitter emitter, string paramName, float paramValue)
    {
        foreach (var VARIABLE in listOfSavedParameterValues)
        {
            if (VARIABLE.Item1 == emitter && VARIABLE.Item2 == paramName)
            {
                Debug.Log("Found one!");
                return VARIABLE.Item3;
            }
        }
        return paramValue;
    }*/
}
