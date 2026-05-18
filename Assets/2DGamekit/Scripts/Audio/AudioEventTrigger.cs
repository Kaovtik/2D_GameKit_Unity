using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioEventTrigger : MonoBehaviour
{
    
    public enum Action
    {
        None,
        Play,
        Stop,
        SetParameter
    }

    [System.Serializable]
    public class AudioSettings
    {
        [HideInInspector]
        public StudioEventEmitter emitter;
        public string tag = "";
        public Action action = Action.None;
        public string parameter = "";
        public float targetValue;
    }

    [NonReorderable] public AudioSettings[] audioSettings;
    
    public void GenerateSettings()
    {
        if (audioSettings.Length != 0)
        {
            int number = 1;
            foreach (AudioSettings i in audioSettings)
            {
                if ( (i.tag == "" && (i.action == Action.Play || i.action == Action.Stop)) || (i.parameter == "" && i.action == Action.SetParameter))
                {
                    Debug.Log("You have unfinished fields in AudioEventSettings number " + number++);
                    
                }
                else
                {
                    switch (i.action)
                    {
                        case Action.None:
                            Debug.Log("You had an AudioEventSetting set to 'None'");
                            break;
                        case Action.Play:
                            i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();
                            if (!i.emitter.EventInstance.isValid())
                                i.emitter.Play();
                            break;
                        case Action.Stop:
                            i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();
                            if (i.emitter.EventInstance.isValid())
                                i.emitter.Stop();
                            break;
                        case Action.SetParameter:
                            RuntimeManager.StudioSystem.getParameterDescriptionByName(i.parameter,
                                out PARAMETER_DESCRIPTION parameterDescription);
                            Debug.Log(i.parameter);
                            Debug.Log(parameterDescription);

                            if (parameterDescription.flags.HasFlag(PARAMETER_FLAGS.GLOBAL))
                            {
                                RuntimeManager.StudioSystem.setParameterByName(i.parameter, i.targetValue);
                                Debug.Log("AudioTriggerSetting number " + number++ + " done. GLOBAL");
                            }
                            else
                            {
                                i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();
                                i.emitter.SetParameter(i.parameter, i.targetValue);
                                Debug.Log("AudioTriggerSetting number " + number++ + " done");
                            }
                                
                            break;
                    }

                    Debug.Log("AudioEventSettings number " + number++ + " done");
                }
            }
        }
        else
            Debug.Log("AudioEventSettings was NULL");
    }
}
