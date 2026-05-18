using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

[CreateAssetMenu(menuName = "Scriptables/Audio/Player")]
public class PlayerAudio : ScriptableObject
{
    [SerializeField] private EventReference playerFootstep;
    [SerializeField] private EventReference playerJump;
    [SerializeField] private EventReference playerLand;
    [SerializeField] private EventReference playerHurt;
    [SerializeField] private EventReference playerMelee;
    [SerializeField] private EventReference playerRanged;
    
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
