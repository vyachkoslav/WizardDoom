using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PauseMenu : MonoBehaviour
{

    private bool isPaused;

    public GameObject pauseMenu;
    public Player.CharacterControls Player;
    public Player.WeaponController Weapon;
    [SerializeField] private InputActionReference pauseAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    void OnEnable()
    {
        pauseAction.action.performed += TogglePause;
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (pauseMenu.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }


    public void PauseGame()
    {
        isPaused = true;
        Player.enabled = false;
        Weapon.enabled = false;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Player.enabled = true;
        Weapon.enabled = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }



    public void OnResumeButtonCliked()
    {
        ResumeGame();//TODO add this to the resume game button
    }
}

