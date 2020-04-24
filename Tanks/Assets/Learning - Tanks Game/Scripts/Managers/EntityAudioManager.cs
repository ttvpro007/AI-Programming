using System.Collections.Generic;
using UnityEngine;

public class EntityAudioManager : MonoBehaviour
{
    public Sound[] m_Sounds = new Sound[1];
    private Dictionary<string, AudioSource> SoundPlayer = new Dictionary<string, AudioSource>();


    private static bool m_DoOnce = false;


    public void Awake()
    {
        SetupSounds();
        SetupSoundPlayer();
    }


    private void SetupSounds()
    {
        foreach (Sound s in m_Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.playOnAwake = s.PlayOnAwake;
            s.Source.loop = s.Loop;
        }
    }


    private void SetupSoundPlayer()
    {
        foreach (Sound s in m_Sounds)
        {
            SoundPlayer.Add(s.Name, s.Source);
        }
    }


    public void Play(string soundName)
    {
        if (!AudioDebug(soundName))
            SoundPlayer[soundName].Play();
    }
    

    public AudioSource GetAudioSource(string soundName)
    {
        AudioDebug(soundName);
        return SoundPlayer[soundName];
    }


    private bool AudioDebug(string soundName)
    {
        if (!SoundPlayer[soundName])
        {
            if (m_DoOnce) return true;

            string message = "No \"" + soundName + "\" name was found in library, please check spelling again.";

            Debug.LogWarning(message);
            m_DoOnce = true;

            return true;
        }

        return false;
    }


    private void UpdateSoundNameInEditor()
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (m_Sounds[i].Clip)
                m_Sounds[i].Name = m_Sounds[i].Clip.name;
            else
                m_Sounds[i].Name = "";
        }
    }


    private void OnValidate()
    {
        UpdateSoundNameInEditor();
    }
}