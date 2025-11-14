using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance { get { return _instance; } }

    private static float sfxvolume = 1;
    public float SFXvolume { get { return sfxvolume; } set { sfxvolume = value; } }


    [SerializeField]
    private SoundLibrary sfxLibary;
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxvolume);
        }
    }


    //Used for  3D sounds, for example spells and shooting
    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibary.GetClipFromName(soundName), pos);
    }

    //Used for 2D sounds, like menu sounds
    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibary.GetClipFromName(soundName), sfxvolume);
    }

        public void UpdateSFXVolume(float value)
    {
        sfxvolume = value;
    }


}