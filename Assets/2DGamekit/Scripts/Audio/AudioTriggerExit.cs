 using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class AudioTriggerExit : MonoBehaviour
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
    public class AudioTriggerExitSettings
    {
        [HideInInspector]
        public StudioEventEmitter emitter;
        public string tag = "";
        public Action action = Action.None;
        public string parameter = "";
        public float targetValue;
    }

    [NonReorderable] public AudioTriggerExitSettings[] audioTriggerExitSettings;

    private BoxCollider2D ownCollider;

    private void Start()
    {
        ownCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioTriggerExitSettings.Length != 1)
            {
                int number = 0;
                foreach (AudioTriggerExitSettings i in audioTriggerExitSettings)
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
                                Debug.Log("AudioTriggerExitSetting number " + number++ + " set to 'None'");
                                break;
                            case Action.Play:
                                i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();
                                Debug.Log("played yes i did");
                                //if (!i.emitter.EventInstance.isValid())
                                i.emitter.Play();
                                Debug.Log("AudioTriggerExitSetting number " + number++ + " done");
                                break;
                            case Action.Stop:
                                i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();
                                if (i.emitter.EventInstance.isValid())
                                    i.emitter.Stop();
                                Debug.Log("AudioTriggerExitSetting number " + number++ + " done");
                                break;
                            case Action.SetParameter:
                                RuntimeManager.StudioSystem.getParameterDescriptionByName(i.parameter,
                                    out PARAMETER_DESCRIPTION parameterDescription);

                                if ((parameterDescription.flags & PARAMETER_FLAGS.GLOBAL) != 0)
                                {
                                    RuntimeManager.StudioSystem.setParameterByName(i.parameter, i.targetValue);
                                    Debug.Log("AudioTriggerExitSetting number " + number++ + " done. GLOBAL");
                                }
                                else
                                {
                                    i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();
                                    i.emitter.SetParameter(i.parameter, i.targetValue);
                                    Debug.Log("AudioTriggerExitSetting number " + number++ + " done");
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
            Debug.Log("AudioTriggerExitSettings was NULL");
    }
}