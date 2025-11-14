using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private static float musicVolume = 1;
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
    
    private static MusicManager _instance;
    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;

    public static MusicManager Instance { get { return _instance; } }

    private void Update()
    {
        musicSource.volume = musicVolume;
    }


    private void Start()
    {

        //This will need to be changed if we add more tracks to the game for now it is enough
        PlayMusic("TestSong");
    }

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
        musicSource.Play();
    }
}