using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class AudioTrigger : MonoBehaviour
{
    public bool destroyAfterUse = true;

    public enum Action
    {
        None,
        Play,
        Stop,
        SetParameter
    }

    [System.Serializable]
    public class AudioTriggerSettings
    {
        [HideInInspector]
        public StudioEventEmitter emitter;
        public string tag = "";
        public Action action = Action.None;
        public string parameter = "";
        public float targetValue;
    }

    [NonReorderable] public AudioTriggerSettings[] audioTriggerSettings;

    private BoxCollider2D ownCollider;

    private void Start()
    {
        ownCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioTriggerSettings.Length != 0)
            {
                int number = 1;
                foreach (AudioTriggerSettings i in audioTriggerSettings)
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
                                Debug.Log("AudioTriggerSetting number " + number++ + " set to 'None'");
                                break;
                            case Action.Play:
                                i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();
                                Debug.Log("played yes i did");
                                //if (!i.emitter.EventInstance.isValid())
                                i.emitter.Play();
                                Debug.Log("AudioTriggerSetting number " + number++ + " done");
                                break;
                            case Action.Stop:
                                i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();
                                if (i.emitter.EventInstance.isValid())
                                    i.emitter.Stop();
                                Debug.Log("AudioTriggerSetting number " + number++ + " done");
                                break;
                            case Action.SetParameter:
                                RuntimeManager.StudioSystem.getParameterDescriptionByName(i.parameter,
                                    out PARAMETER_DESCRIPTION parameterDescription);

                                if ((parameterDescription.flags & PARAMETER_FLAGS.GLOBAL) != 0)
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
                    }
                }
            }
            if (destroyAfterUse)
            {
                Debug.Log("Destroyed");
                ownCollider.enabled = false;
            }
        }
        else
            Debug.Log("AudioTriggerSettings was NULL");
    }
}
