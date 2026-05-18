using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Gamekit2D
{
    [RequireComponent(typeof(Slider))]
    public class MixerSliderLink : MonoBehaviour
    {
        [SerializeField] private string vcaName;
        private VCA vca;

        protected Slider m_Slider;


        void Awake ()
        {
            if (vcaName == "")
            {
                return;
            }
            
            m_Slider = GetComponent<Slider>();
            vca = RuntimeManager.GetVCA("vca:/" + vcaName);

            float value;
            vca.getVolume(out value);
            
            m_Slider.value = value;

            m_Slider.onValueChanged.AddListener(SliderValueChange);
        }


        void SliderValueChange(float value)
        {
            if (vcaName == "")
            {
                return;
            }
            vca.setVolume(value);
        }
    }
}