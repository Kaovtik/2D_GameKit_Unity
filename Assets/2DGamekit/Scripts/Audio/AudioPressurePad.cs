using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPressurePad : MonoBehaviour
{
    public void PlayPressureP()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayPressurePad();
    }
}