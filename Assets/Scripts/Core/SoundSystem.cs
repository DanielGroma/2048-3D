using UnityEngine;
using System;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private Sound[] _sounds;

    [Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
        [Range(0.1f, 3f)] public float Pitch = 1f;
        public bool Loop = false;
        [HideInInspector] public AudioSource Source;
    }

    private void Awake()
    {
        foreach (var sound in _sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
        }
    }

    public void PlaySound(string name)
    {
        var sound = Array.Find(_sounds, s => s.Name == name);
        if (sound != null)
            sound.Source.Play();
        else
            Debug.LogWarning($"Sound {name} not found");
    }
}