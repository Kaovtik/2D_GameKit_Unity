using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public float combatTimerLength = 0f;

    [System.Serializable]
    public struct Emitters
    {
        public StudioEventEmitter musicMenu;
        public StudioEventEmitter music;
        public StudioEventEmitter music02;
        public StudioEventEmitter musicBoss;
        public StudioEventEmitter musicPause;
        public StudioEventEmitter snapShotPause;
    }
    public Emitters eventEmitters;

    [Space(10)]

    [Header("Player")]
    [SerializeField] private EventReference playerFootsteps;
    [SerializeField] private EventReference playerJump;
    [SerializeField] private EventReference playerLand;
    [SerializeField] private EventReference playerAttackMelee;
    
    [SerializeField] private EventReference playerHurt;
    EventInstance playerFootstepInstance;
    EventInstance playerLandInstance;

    [Header("Objects & Interactables")]
    [SerializeField] private EventReference wallDestroy;
    [SerializeField] private EventReference pickupHealth;
    [SerializeField] private EventReference doorSwitch;
    [SerializeField] private EventReference doorOpen;
    [SerializeField] private EventReference pickupKey;
    [SerializeField] private EventReference pressurePad;
    [SerializeField] private EventReference acidSplash;
    [SerializeField] private EventReference boxPush;
    EventInstance boxPushInstance;

    [Header("UI")]
    [SerializeField] private EventReference menuClick;
    [SerializeField] private EventReference menuStartGame;
    
    [Header("Enemies")]
    [SerializeField] private EventReference enemyFootstep;
    [SerializeField] private EventReference enemyRanged;
    [SerializeField] private EventReference enemyMelee;
    [SerializeField] private EventReference enemyDeath;
    private EventInstance enemyFootstepInstance;

    [Header("Stingers")]
    [SerializeField] private EventReference stingerGameOver;
    [SerializeField] private EventReference stingerKeyPickup;
    [SerializeField] private EventReference stingerWeaponPickup;
        
    [HideInInspector]
    public bool combatState;
    private bool timerRunning;
    private int killedEnemies = 0;
    private string killedEnemiesParam = "EnemiesKilled";
    private float comboTimer = 0f, comboFullTimer = 5f;

    public string aggroEnemyParamName = "";
    public int aggroEnemyCount = 0;

    public List<Tuple<StudioEventEmitter, string, float>> savedParameterValues =
        new List<Tuple<StudioEventEmitter, string, float>>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
    
    public void Health(int currentHealth)
    {
        eventEmitters.music02.SetParameter("Health", currentHealth);
        Debug.Log("Health is: " + currentHealth);
    }

    private void Update()
    {
        comboTimer -= Time.deltaTime;
        
        if (comboTimer <= 0)
        {
            killedEnemies = 0;
            RuntimeManager.StudioSystem.setParameterByName(killedEnemiesParam, killedEnemies);
        }

        Debug.Log(aggroEnemyCount);
        
        //RuntimeManager.StudioSystem.setParameterByName(aggroEnemyParamName, aggroEnemyCount);
    }

    public void CombatState(bool combatS)
    {
        combatState = combatS;
        if (combatState == true && timerRunning == false)
        {
            StartCoroutine(CombatTimer());
        }
        if (combatState == true && timerRunning == true)
        {
            StopAllCoroutines();
            timerRunning = false;
            Debug.Log("Timer off 2");
            StartCoroutine(CombatTimer());
        }
    }

    private IEnumerator CombatTimer()
    {
        /*if (eventEmitters.music.EventInstance.isValid())
            eventEmitters.music.SetParameter("Combat", 1f);
            */
        if (eventEmitters.music02.EventInstance.isValid())
            eventEmitters.music02.SetParameter("Combat", 1f);
        
        Debug.Log("Timer on");
        timerRunning = true;
        float timer = combatTimerLength;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        combatState = false;
        timerRunning = false;
        /*if(eventEmitters.music.EventInstance.isValid())
            eventEmitters.music.SetParameter("Combat", 0f);
            */
        if (eventEmitters.music02.EventInstance.isValid())
            eventEmitters.music02.SetParameter("Combat", 0f);
        Debug.Log("Timer off");
    }

    public void PlayFootstep(string surface)
    {
        if (playerFootsteps.IsNull)
        {
            Debug.LogWarning("Fmod event not found: playerFootstep");
            return;
        }
        playerFootstepInstance = RuntimeManager.CreateInstance(playerFootsteps);
        switch(surface)
        {
            case "Grass":
                playerFootstepInstance.setParameterByName("Surface", 0f);
                break;
            case "Rock":
                playerFootstepInstance.setParameterByName("Surface", 1f);
                break;
            case "Metal":
                playerFootstepInstance.setParameterByName("Surface", 2f);
                break;
            default:
                playerFootstepInstance.setParameterByName("Surface", 0f);
                break;
        }
        playerFootstepInstance.start();
        playerFootstepInstance.release();
    }

    public void PlayJump()
    {
        if (playerJump.IsNull)
        {
            Debug.LogWarning("Fmod event not found: playerJump");
            return;
        }
        RuntimeManager.PlayOneShot(playerJump);
    }

    public void PlayLand(string surface)
    {
        if (playerLand.IsNull)
        {
            Debug.LogWarning("Fmod event not found: playerLand");
            return;
        }
        playerLandInstance = RuntimeManager.CreateInstance(playerLand);
        switch(surface)
        {
            case "Grass":
                playerFootstepInstance.setParameterByName("Surface", 0f);
                break;
            case "Rock":
                playerFootstepInstance.setParameterByName("Surface", 1f);
                break;
            case "Metal":
                playerFootstepInstance.setParameterByName("Surface", 2f);
                break;
            default:
                playerFootstepInstance.setParameterByName("Surface", 0f);
                break;
        }
        playerLandInstance.start();
        playerLandInstance.release();
    }

	public void PlayMelee()
	{
        if (playerAttackMelee.IsNull)
        {
            Debug.LogWarning("Fmod event not found: playerAttackMelee");
            return;
        }
        RuntimeManager.PlayOneShot(playerAttackMelee, transform.position);
    }

	

    public void PlayHurt()
    {
        if (playerHurt.IsNull)
        {
            Debug.LogWarning("Fmod event not found: playerHurt");
            return;
        }
        RuntimeManager.PlayOneShot(playerHurt, transform.position);
    }

    public void PlayDestroy(GameObject destroyObject)
    {
        if (wallDestroy.IsNull)
        {
            Debug.LogWarning("Fmod event not found: wallDestroy");
            return;
        }
        RuntimeManager.PlayOneShot(wallDestroy, destroyObject.transform.position);
    }

    public void PlayDoor(GameObject doorObject)
    {
        if (doorOpen.IsNull)
        {
            Debug.LogWarning("Fmod event not found: doorOpen");
            return;
        }
        RuntimeManager.PlayOneShot(doorOpen, doorObject.transform.position);
    }

    public void PlayBoxPush(bool pushing = false)
    {
        if (boxPush.IsNull)
        {
            Debug.LogWarning("Fmod event not found: boxPush");
            return;
        }
        if (pushing)
        {
            boxPushInstance = RuntimeManager.CreateInstance(boxPush);
            boxPushInstance.start();
            Debug.Log("Started Pushing");
        }
        else
        {
            boxPushInstance.setParameterByName("End", 1f);
            boxPushInstance.release();
            Debug.Log("Stopped Pushing");
        }
    }

    public void PlayHealth()
    {
        if (pickupHealth.IsNull)
        {
            Debug.LogWarning("Fmod event not found: pickupHealth");
            return;
        }
        RuntimeManager.PlayOneShot(pickupHealth);
    }

    public void PlayDoorSwitch(GameObject switchObject)
    {
        if (doorSwitch.IsNull)
        {
            Debug.LogWarning("Fmod event not found: doorSwitch");
            return;
        }
        RuntimeManager.PlayOneShotAttached(doorSwitch, switchObject);
        Debug.Log("Played DoorSwitch");
    }

    public void PlayKey(GameObject keyObject)
    {
        RuntimeManager.PlayOneShotAttached(pickupKey, keyObject);

        if (stingerKeyPickup.IsNull)
        {
            Debug.LogWarning("Fmod event not found: stingerKeyPickup");
            return;
        }
        RuntimeManager.PlayOneShotAttached(stingerKeyPickup, keyObject);
        Debug.Log("Played stingerKeyPickup");
    }

    public void PlayWeaponPickup()
    {
        RuntimeManager.PlayOneShot(stingerWeaponPickup);
    }
    
    public void PlayGameOver()
    {
        if (stingerGameOver.IsNull)
        {
            Debug.LogWarning("Fmod event not found: stingerGameOver");
            return;
        }
        RuntimeManager.PlayOneShot(stingerGameOver);
    }

    public void PlayPressurePad()
    {
        if (pressurePad.IsNull)
        {
            Debug.LogWarning("Fmod event not found: pressurePad");
            return;
        }
        RuntimeManager.PlayOneShot(pressurePad);
    }

    public void PlayMenuGeneric()
    {
        if (menuClick.IsNull)
        {
            Debug.LogWarning("Fmod event not found: menuClick");
            return;
        }
        RuntimeManager.PlayOneShot(menuClick);
    }

    public void PlayMenuStart()
    {
        if (menuStartGame.IsNull)
        {
            Debug.LogWarning("Fmod event not found: menuStartGame");
            return;
        }
        RuntimeManager.PlayOneShot(menuStartGame);
    }

    public void PlaySplash(GameObject splashObject)
    {
        if (acidSplash.IsNull)
        {
            Debug.LogWarning("Fmod event not found: acidSplash");
            return;
        }
        RuntimeManager.PlayOneShotAttached(acidSplash, splashObject);
    }
    public void PlayEnemyFootstep(GameObject enemyObject, int walkType)
    {
        if (enemyFootstep.IsNull)
        {
            Debug.LogWarning("Fmod event not found: enemyWalk");
            return;
        }

        switch (walkType)
        {
            case 0:
                enemyFootstepInstance = RuntimeManager.CreateInstance(enemyFootstep);
                RuntimeManager.AttachInstanceToGameObject(enemyFootstepInstance, enemyObject.transform);
                enemyFootstepInstance.setParameterByName("Run", 0f);
                enemyFootstepInstance.start();
                enemyFootstepInstance.release();
                break;
            case 1:
                enemyFootstepInstance = RuntimeManager.CreateInstance(enemyFootstep);
                RuntimeManager.AttachInstanceToGameObject(enemyFootstepInstance, enemyObject.transform);
                enemyFootstepInstance.setParameterByName("Run", 1f);
                enemyFootstepInstance.start();
                enemyFootstepInstance.release();
                break;
        }
    }
    public void PlayEnemyRanged(GameObject enemyObject)
    {
        if (enemyRanged.IsNull)
        {
            Debug.LogWarning("Fmod event not found: enemyRanged");
            return;
        }
        RuntimeManager.PlayOneShotAttached(enemyRanged, enemyObject);
    }

    public void PlayEnemyMelee(GameObject enemyObject)
    {
        if (enemyMelee.IsNull)
        {
            Debug.LogWarning("Fmod event not found: enemyMelee");
            return;
        }
        RuntimeManager.PlayOneShotAttached(enemyMelee, enemyObject);
    }

    public void PlayEnemyDeath(GameObject enemyObject)
    {
        if (enemyDeath.IsNull)
        {
            Debug.LogWarning("Fmod event not found: enemyDeath");
            return;
        }
        RuntimeManager.PlayOneShotAttached(enemyDeath, enemyObject);
    }

    public void PlayMusicPause()
    {
        if (eventEmitters.musicPause.EventReference.IsNull)
        {
            Debug.LogWarning("Fmod event not found: eventEmitters.musicPause");
            return;
        }

        if (!eventEmitters.musicPause.IsActive)
        {
            eventEmitters.musicPause.Play();
        }
        
        if (eventEmitters.snapShotPause.EventReference.IsNull)
        {
            Debug.LogWarning("Fmod event not found: eventEmitters.musicPause");
            return;
        }
        
        if (!eventEmitters.snapShotPause.IsActive)
        {
            eventEmitters.snapShotPause.Play();
        }
        
    }

    public void StopMusicPause()
    {
        if (eventEmitters.musicPause.EventReference.IsNull)
        {
            Debug.LogWarning("Fmod event not found: eventEmitters.musicPause");
            return;
        }

        if (eventEmitters.musicPause.IsActive)
        {
            eventEmitters.musicPause.Stop();
        }
        
        if (eventEmitters.snapShotPause.EventReference.IsNull)
        {
            Debug.LogWarning("Fmod event not found: eventEmitters.musicPause");
            return;
        }
        
        if (eventEmitters.snapShotPause.IsActive)
        {
            eventEmitters.snapShotPause.Stop();
        }
    }

    public void IncrementEnemyKills()
    {
        comboTimer = comboFullTimer;
        killedEnemies++;
        RuntimeManager.StudioSystem.setParameterByName(killedEnemiesParam, killedEnemies);
    }

    public float SaveParameterValue(StudioEventEmitter emitter, string parameter, float parameterValue)
    {
        Tuple<StudioEventEmitter, string, float> savedParam =
            new Tuple<StudioEventEmitter, string, float>(emitter, parameter, parameterValue);

        if (savedParameterValues.Count == 0)
        {
            savedParameterValues.Add(savedParam);
            return savedParam.Item3;
        }
        
        for (int i = 0; i < savedParameterValues.Count; i++)
        {
            if (savedParameterValues[i].Item1 == emitter && savedParameterValues[i].Item2 == parameter)
            {
                if (savedParameterValues[i].Item3 < parameterValue)
                {
                    savedParameterValues.Insert(i, savedParam);
                    return savedParam.Item3;
                }
                else
                {
                    return savedParameterValues[i].Item3;
                }
            }
        }
        savedParameterValues.Add(savedParam);
        return savedParam.Item3;
    }
}