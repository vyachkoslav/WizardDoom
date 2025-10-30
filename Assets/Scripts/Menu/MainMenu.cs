using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        //Put the Game Music List Here
        MusicManager.Instance.PlayMusic("TestSong");
    }

    public void StartGame()
    {
        //Load the first level here for now the SampleScene is used
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    //TODO when audio manager is done add the code for managing sound sliders here
}
