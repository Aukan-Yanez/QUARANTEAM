using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public string testAudioName = "default";
    public Sound[] sounds;
    public Slider volume;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
        play(testAudioName);
        sounds[0].audioSource.volume = PlayerPrefs.GetFloat("gralVolume");
        volume.value = PlayerPrefs.GetFloat("gralVolume");
    }
    public void play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Audio clip '" + name + "', no ha sido encontrado.");
            return;
        }
        s.audioSource.Play();
    }
    void Update()
    {
        foreach (Sound s in sounds)
        {
            s.audioSource.volume = s.volume;
        }
    }
    public void setVolume(float newVolume)
    {
        foreach (Sound s in sounds)
        {
            s.volume = newVolume;
            PlayerPrefs.SetFloat("gralVolume",newVolume);
        }
    }
}
