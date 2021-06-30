using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Sound[] sounds;
    public Sound[] triggerIndependentSounds;
    public Sound[] moveSounds;
    public float minTimeBetweenAmbient;
    public float maxTimeBetweenAmbient;

    Sound currentAmbient;

    private void Awake()
    {
        App.audioManager = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        foreach (Sound s in triggerIndependentSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        foreach (Sound s in moveSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        LoadVolumeValues();
    }

    public void Play(string name)
    {
        Sound s = FindSound(name);
        s.source.PlayOneShot(s.clip);
    }

    public void PlayLoop(string name)
    {
        Sound s = FindSound(name);
        if (s.source.isPlaying) return;
        s.source.Play();
    }

    public void PlayMoveSound()
    {
        Debug.Log(moveSounds.Length);
        int rnd = UnityEngine.Random.Range(0, moveSounds.Length);
        moveSounds[rnd].source.PlayOneShot(moveSounds[rnd].clip);
    }

    public void Stop(string name)
    {
        Sound s = FindSound(name);
        s.source.Stop();
    }

    public Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio manager invalid sound name: " + name);
            return null;
        }
        else
            return s;
    }

    public IEnumerator PlayAmbient()
    {
        Debug.Log("Ambient played");

        while (true)
        {
            currentAmbient = triggerIndependentSounds[UnityEngine.Random.Range(0, triggerIndependentSounds.Length)];
            currentAmbient.source.Play();
            yield return new WaitForSeconds(currentAmbient.source.clip.length + UnityEngine.Random.Range(minTimeBetweenAmbient, maxTimeBetweenAmbient)); 
        }
    }

    public void StopAmbient()
    {
        Debug.Log("Ambient stopped");

        StopCoroutine(PlayAmbient());
        currentAmbient.source.Stop();
    }

    void LoadVolumeValues()
    {
        LoadValue("masterVol");
        LoadValue("sfxVol");
        LoadValue("ambientVol");
        LoadValue("musicVol");
    }

    void LoadValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
            mainMixer.SetFloat(key, Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat(key), 0.0001f)) * 20f);
        else
            mainMixer.SetFloat(key, Mathf.Log10(0.5f) * 20f);
    }


    public IEnumerator PlayIngameMusic()
    {
        Debug.Log("IngameMusic played");

        while (true)
        {
            currentAmbient = triggerIndependentSounds[0];
            currentAmbient.source.Play();
            yield return new WaitForSeconds(currentAmbient.source.clip.length);
            currentAmbient = triggerIndependentSounds[1];
            currentAmbient.source.Play();
            yield return new WaitForSeconds(currentAmbient.source.clip.length);
            currentAmbient = triggerIndependentSounds[2];
            currentAmbient.source.Play();
            yield return new WaitForSeconds(currentAmbient.source.clip.length);
            currentAmbient = triggerIndependentSounds[0];
            currentAmbient.source.Play();
            yield return new WaitForSeconds(currentAmbient.source.clip.length);
            currentAmbient = triggerIndependentSounds[1];
            currentAmbient.source.Play();
            yield return new WaitForSeconds(currentAmbient.source.clip.length);
        }
    }

    public void StopIngameMusic()
    {
        Debug.Log("IngameMusic stopped");

        StopCoroutine(PlayIngameMusic());
        currentAmbient.source.Stop();
    }

}