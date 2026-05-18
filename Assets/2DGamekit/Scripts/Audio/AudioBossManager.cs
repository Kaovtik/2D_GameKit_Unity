using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using UnityEngine.Serialization;

public class AudioBossManager : MonoBehaviour
{
    [Header("Event Instances")]
    [SerializeField] private EventReference bossWalk;
    [SerializeField] private EventReference bossLaserAttack;
    [SerializeField] private EventReference bossGrenadeAttack;
    [SerializeField] private EventReference bossLightningAttack;
    [SerializeField] private EventReference bossTakeDamage;
    [SerializeField] private EventReference bossShield;
    [SerializeField] private EventReference bossSteamStage;
    [SerializeField] private EventReference bossDie;

    [Header("Music Settings")]
    public string bossMusic = "";
    public string stageParameter = "";
    public float stage1Value = 0f;
    public float stage2Value = 0f;
    public float bossDeathValue = 0f;
    public string stunParameter = "";

    private StudioEventEmitter bossMusicEmitter;
    private EventInstance eventInstance;

    private void Start()
    {
        bossMusicEmitter = GameObject.FindGameObjectWithTag(bossMusic).GetComponent<StudioEventEmitter>();
    }

    public void BossWalk(GameObject boss)
    {
        if (bossWalk.IsNull)
        {
            Debug.LogWarning("Fmod event not found: bossWalk");
            return;
        }
        RuntimeManager.PlayOneShotAttached(bossWalk, boss);
    }

    public void BossLaserAttack(GameObject laserObj)
    {
        if (bossLaserAttack.IsNull)
        {
            Debug.LogWarning("Fmod event not found: bossLaserAttack");
            return;
        }
        RuntimeManager.PlayOneShotAttached(bossLaserAttack, laserObj);
    }

    public void BossGrenadeAttack(GameObject grenadeObj)
    {
        if (bossGrenadeAttack.IsNull)
        {
            Debug.LogWarning("Fmod event not found: bossGrenadeAttack");
            return;
        }
        RuntimeManager.PlayOneShotAttached(bossGrenadeAttack, grenadeObj);
    }

    public void BossLightningAttack(GameObject lightningObj)
    {
        if (bossLightningAttack.IsNull)
        {
            Debug.LogWarning("Fmod event not found: bossLightningAttack");
            return;
        }
        RuntimeManager.PlayOneShotAttached(bossLightningAttack, lightningObj);
    }

    public void BossTakeDamage(GameObject boss)
    {
        if (bossTakeDamage.IsNull)
        {
            Debug.LogWarning("Fmod event not found: bossTakeDamage");
            return;
        }
        RuntimeManager.PlayOneShotAttached(bossTakeDamage, boss);
    }

    public void BossShield(GameObject boss, int shieldState)
    {
        if (bossShield.IsNull)
        {
            Debug.LogWarning("Fmod event not found: bossShield");
            return;
        }
        switch (shieldState)
        {
            case 0:
                Debug.Log("shield state 0");
                eventInstance = RuntimeManager.CreateInstance(bossShield);
                RuntimeManager.AttachInstanceToGameObject(eventInstance, boss.transform);
                eventInstance.setParameterByName("ShieldState", 0f);
                eventInstance.start();
                eventInstance.release();
                bossMusicEmitter.SetParameter(stunParameter, 0f);
                break;
            case 1:
                eventInstance = RuntimeManager.CreateInstance(bossShield);
                RuntimeManager.AttachInstanceToGameObject(eventInstance, boss.transform);
                eventInstance.setParameterByName("ShieldState", 1f);
                eventInstance.start();
                eventInstance.release();
                bossMusicEmitter.SetParameter(stunParameter, 1f);
                break;
        }
    }
    
    public void BossSteamStage(GameObject boss, int steamStage)
    {
        switch (steamStage)
        {
            case 1:
                bossMusicEmitter.SetParameter(stageParameter, stage1Value);
                if (bossSteamStage.IsNull)
                {
                    Debug.LogWarning("Fmod event not found: bossSteamStage");
                    return;
                }
                eventInstance = RuntimeManager.CreateInstance(bossSteamStage);
                RuntimeManager.AttachInstanceToGameObject(eventInstance, boss.transform);
                eventInstance.setParameterByName("SteamStage", 1f);
                eventInstance.start();
                eventInstance.release();
                break;
            case 2:
                bossMusicEmitter.SetParameter(stageParameter, stage2Value);
                if (bossSteamStage.IsNull)
                {
                    Debug.LogWarning("Fmod event not found: bossSteamStage");
                    return;
                }
                eventInstance = RuntimeManager.CreateInstance(bossSteamStage);
                RuntimeManager.AttachInstanceToGameObject(eventInstance, boss.transform);
                eventInstance.setParameterByName("SteamStage", 2f);
                eventInstance.start();
                eventInstance.release();
                break;
        }
    }
    
    public void BossDie(GameObject boss)
    {
        if (bossDie.IsNull)
        {
            Debug.LogWarning("Fmod event not found: bossDie");
            bossMusicEmitter.SetParameter(stunParameter, 1f);
            bossMusicEmitter.SetParameter(stageParameter, bossDeathValue);
            return;
        }
        RuntimeManager.PlayOneShotAttached(bossDie, boss);
        bossMusicEmitter.SetParameter(stunParameter, 1f);
        bossMusicEmitter.SetParameter(stageParameter, bossDeathValue);
    }
}