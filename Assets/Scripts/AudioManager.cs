using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {

    public Sound[] Sounds;

    public static AudioManager instance;

	// Use this for initialization
	void Awake () {

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

		foreach(Sound s in Sounds)
        {
            s.audioSrc = gameObject.AddComponent<AudioSource>();
            s.audioSrc.clip = s.clip;
            s.audioSrc.volume = s.volume;
            s.audioSrc.pitch = s.pitch;
        }
	}
	
	public void Play(string nameOfSound)
    {
        Sound s = Array.Find(Sounds, Sound => Sound.name == nameOfSound);
        if(s == null)
        {
            Debug.LogWarning("Sound " + nameOfSound + ": is null, check for spelling error or wrong usage");
            return;
        }
        s.audioSrc.Play();
    }
}
