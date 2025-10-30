using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private static MusicManager _instance;
    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;

    public static MusicManager Instance { get { return _instance; } }

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

    public void PlayMusic(string trackName)
    {
        AudioClip nextTrack = musicLibrary.GetClipFromName(trackName);

        if (nextTrack == null)
        {
            return;
        }

        musicSource.clip = nextTrack;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }
}