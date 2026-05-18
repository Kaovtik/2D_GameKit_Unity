using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHealth : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayHealth()
    {
        audioManager.PlayHealth();
    }
}
