using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;


    void Start()
    {
        musicSlider.value = MusicManager.Instance.MusicVolume;
        musicSlider.onValueChanged.AddListener(delegate { MusicManager.Instance.MusicVolume = musicSlider.value; });

        sfxSlider.value = SoundManager.Instance.SFXvolume;
        sfxSlider.onValueChanged.AddListener(delegate { SoundManager.Instance.SFXvolume = sfxSlider.value; });
    }

}
