using FMODUnity;
using UnityEngine;

public class AudioGenericPickup : MonoBehaviour
{
    
    public void PlayDoorSwitch()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayDoorSwitch(gameObject);
        //RuntimeManager.PlayOneShot(audioManager.doorSwitch, transform.position);
    }
    
    public void PlayKey()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayKey(gameObject);
    }

    public void PlayWeaponPickup()
    {
        AudioManager.Instance.PlayWeaponPickup();
    }
}
