using System;
using System.Collections.Generic;
using UnityEngine;


// expose enum list for sound clips
public enum AudioKey
{
    PlayerFiresProjectileWeapon,
    PlayerHitByAttackGrunt,
    BlockedDamage,
    EnemyDamaged
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    // public AudioClip playerFiresProjectileWeapon; // player fires projectile weapon
    // public AudioClip playerHitByAttackGrunt; // player takes hit by enemy attack

    [Serializable]
    public struct AudioMapping
    {
        public AudioKey key; // Now using the Enum
        public AudioClip clip;
    }

    [Header("Audio Settings")]
    [SerializeField] public List<AudioMapping> audioMappings;

    // Dictionary now maps Enum -> AudioClip
    private Dictionary<AudioKey, AudioClip> audioDictionary;


    // Audio sources
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicate AudioManagers
            return;
        }
        Instance = this; // expose instance to all classes


        audioDictionary = new Dictionary<AudioKey, AudioClip>();
        foreach (var mapping in audioMappings)
        {
            if (!audioDictionary.ContainsKey(mapping.key))
            {
                audioDictionary.Add(mapping.key, mapping.clip);
            }
            else
            {
                Debug.LogWarning($"Duplicate AudioKey found: {mapping.key}. Only the first one will be used.");
            }
        }
    }

    // Now type-safe! No more string typos.
    public void PlayAudioClip(AudioKey key)
    {
        if (audioDictionary.TryGetValue(key, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Audio clip for {key} is missing in the inspector!");
        }
    }
}
