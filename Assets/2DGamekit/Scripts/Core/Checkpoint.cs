using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Gamekit2D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Checkpoint : MonoBehaviour
    {
        public bool respawnFacingLeft;

        private StudioEventEmitter emitter;
        public string eventTag = "";
        public string parameter = "";
        public float targetValue = 0f;

        private void Reset()
        {
            GetComponent<BoxCollider2D>().isTrigger = true; 
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerCharacter c = collision.GetComponent<PlayerCharacter>();
            if(c != null)
            {
                c.SetChekpoint(this);
            }
        }

        public void SetParameterOnRespawn()
        {
            //emitter = GameObject.FindGameObjectWithTag(eventTag).GetComponent<StudioEventEmitter>();
            //emitter.SetParameter(parameter, targetValue);
        }

        public void SetParameterOnGameOver()
        {
            if (eventTag == "")
            {
                Debug.Log($"No event tag in {gameObject} for game over settings");
                return;
            }
            emitter = GameObject.FindGameObjectWithTag(eventTag).GetComponent<StudioEventEmitter>();
            AudioZoneSettings audioZoneSettings = FindObjectOfType<AudioZoneSettings>();

            foreach (AudioZoneSettings.AudioSettings i in audioZoneSettings.audioSettings)
            {
                if (i.action == AudioZoneSettings.Action.SetParameter && i.tag == eventTag)
                {
                    targetValue =  i.targetValue;
                    emitter.SetParameter(parameter, targetValue);
                }
            }
        }
    }
}