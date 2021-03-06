using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public static class Sound_Events
{
    #region play sound event
    public static event System.Action<string> play_sound_event;
    public static void Play_Sound(string sound_name)
    {
        play_sound_event?.Invoke(sound_name);
    }
    #endregion
    #region delay play sound event
    public static event System.Action<string, float> delay_play_sound_event;
    public static void Delay_Play_Sound(string sound_name, float delay)
    {
        delay_play_sound_event?.Invoke(sound_name, delay);
    }
    #endregion
    #region stop sound event
    public static event System.Action<string> stop_sound_event;
    public static void Stop_Sound(string sound_name)
    {
        stop_sound_event?.Invoke(sound_name);
    }
    #endregion
    #region change volume sound event
    public static event System.Action<float, Sound_Type_Tags> change_volume_event;
    public static void Change_Volume(float new_volume, Sound_Type_Tags tag)
    {
        change_volume_event?.Invoke(new_volume, tag);
    }
    #endregion
}

public enum Sound_Type_Tags
{
    music,
    fx,
    menu
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public Sound_Type_Tags Tag;

    public bool loop;
    public bool randomPitch;
    [Range(0, 1)]
    public float Maxvolume;
    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}

public class Audio_Controller_Script : MonoBehaviour
{
    [Range(0, 1)]
    public float currentGameVolumeLevel;
    [Range(0, 1)]
    public float currentMusicVolumeLevel;
    [Range(0, 1)]
    public float currentMenuVolumeLevel;
    public Sound[] Sounds;
    public Dictionary<string, AudioSource> MusicAS = new Dictionary<string, AudioSource>();
    public Dictionary<string, AudioSource> FXAS = new Dictionary<string, AudioSource>();
    public Dictionary<string, AudioSource> MenuAS = new Dictionary<string, AudioSource>();
    public static Audio_Controller_Script singleton;
    public AudioSource selectedSound;

    private IEnumerator _IfadeIn;
    private IEnumerator _IfadeOut;

    private void Awake()
    {
        //Checks to make sure there is only one SM
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.Maxvolume;
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            switch (s.Tag)
            {
                case Sound_Type_Tags.music:
                    s.source.volume = currentMusicVolumeLevel;
                    MusicAS[s.name] = s.source;
                    break;
                case Sound_Type_Tags.fx:
                    s.source.volume = currentGameVolumeLevel;
                    FXAS[s.name] = s.source;
                    break;
                case Sound_Type_Tags.menu:
                    s.source.volume = currentMenuVolumeLevel;
                    MenuAS[s.name] = s.source;
                    break;
                default:
                    break;
            }
            s.source.playOnAwake = false;
            s.source.pitch = s.pitch;
        }

        Sound_Events.play_sound_event += PlaySound;
        Sound_Events.delay_play_sound_event += DelayPlaySound;
        Sound_Events.stop_sound_event += StopSound;
        Sound_Events.change_volume_event += ChangeVolume;

    }

    public void ChangeVolume(float newVolume, Sound_Type_Tags tag)
    {
        //float volumeDelta = newVolume - currentVolumeLevel;

        foreach (Sound s in Sounds)
        {
            if (s.Tag == tag)
            {
                s.source.volume = s.Maxvolume * newVolume;
            }
        }
        switch (tag)
        {
            case Sound_Type_Tags.music:
                currentMusicVolumeLevel = newVolume;
                break;
            case Sound_Type_Tags.fx:
                currentGameVolumeLevel = newVolume;
                break;
            case Sound_Type_Tags.menu:
                currentMenuVolumeLevel = newVolume;
                break;
            default:
                print("incorrect volume change tag sent");
                break;
        }

    }


    public void PlaySound(string soundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log(soundName + " sound not found!");
            return;
        }
        if (s.randomPitch)
        {
            s.source.pitch = Random.Range(s.pitch - 0.2f, s.pitch + 0.2f);
        }
        s.source.Play();
    }

    public void DelayPlaySound(string soundName, float delayTime)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log(soundName + " sound not found!");
            return;
        }
        if (s.randomPitch)
        {
            s.source.pitch = Random.Range(s.pitch - 0.2f, s.pitch + 0.2f);
        }

        IEnumerator DelayPlay()
        {
            yield return new WaitForSeconds(delayTime);
            s.source.Play();
        }
        StartCoroutine(DelayPlay());
        
    }

    public void StopSound(string soundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log(soundName + " sound not found!");
            return;
        }
        s.source.Stop();
    }

    public void SelectSound(string soundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.name == soundName);
        selectedSound = s.source;
    }

    public void ChangeSelectedSoundVolume(float newVolume)
    {
        selectedSound.volume = newVolume;
    }

    public void FadeSoundIn(float fadeMax, float speed)
    {
        if(_IfadeIn != null){StopCoroutine(_IfadeIn);}
        if(_IfadeOut != null){StopCoroutine(_IfadeOut);}
        
        Sound s = System.Array.Find(Sounds, sound => sound.source == selectedSound);
        s.source.volume = 0f;

        IEnumerator IFadeIn()
        {
            while(s.source.volume < fadeMax)
            {
                s.source.volume += speed;
                yield return new WaitForSeconds(0.05f);
            }
        }
        _IfadeIn = IFadeIn();
        StartCoroutine(IFadeIn());
    }
    public void FadeSoundOut(float speed)
    {
        if(_IfadeIn != null){StopCoroutine(_IfadeIn);}
        if(_IfadeOut != null){StopCoroutine(_IfadeOut);}

        Sound s = System.Array.Find(Sounds, sound => sound.source == selectedSound);

        IEnumerator IFadeOut()
        {
            while(s.source.volume > 0)
            {
                s.source.volume -= speed;
                yield return new WaitForSeconds(0.05f);
            }
        }
        _IfadeOut = IFadeOut();
        StartCoroutine(IFadeOut());
    }

    private void OnDestroy()
    {
        if(_IfadeIn != null){StopCoroutine(_IfadeIn);}
        if(_IfadeOut != null){StopCoroutine(_IfadeOut);}

        Sound_Events.play_sound_event -= PlaySound;
        Sound_Events.delay_play_sound_event -= DelayPlaySound;
        Sound_Events.stop_sound_event -= StopSound;
        Sound_Events.change_volume_event -= ChangeVolume;
    }
}
