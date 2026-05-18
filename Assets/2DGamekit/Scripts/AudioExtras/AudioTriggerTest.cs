using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioTriggerTest : MonoBehaviour
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
        public bool saveParam = false;
        public bool checkParam = false;
        public bool incrementParam = false;
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
                    if ((i.parameter == "" && i.action == Action.SetParameter) || i.tag == "")
                    {
                        Debug.Log("You have unfinished fields in AudioTriggerSettings number " + number++);

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
                                
                                i.emitter = GameObject.FindGameObjectWithTag(i.tag).GetComponent<StudioEventEmitter>();

                                if (i.saveParam)
                                {
                                    AudioManager.Instance.SaveParameterValue(i.emitter, i.parameter, i.targetValue);
                                }

                                if (i.incrementParam)
                                {
                                    float previousValue = AudioManager.Instance.SaveParameterValue(i.emitter, i.parameter, 0);
                                    i.targetValue = previousValue;
                                    i.targetValue++;
                                    AudioManager.Instance.SaveParameterValue(i.emitter, i.parameter, i.targetValue);
                                }
                                
                                i.emitter.SetParameter(i.parameter, i.targetValue);
                                Debug.Log("AudioTriggerSetting number " + number++ + " done. Value: " + i.targetValue);
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
