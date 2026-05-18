using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDoor : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void PlayDoor()
    {
        audioManager.PlayDoor(gameObject);
    }
}
