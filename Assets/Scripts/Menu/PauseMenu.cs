using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public Player.CharacterControls Player;
    [SerializeField] private InputActionReference pauseAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void OnEnable()
    {
        pauseAction.action.performed += PauseGame;
        pauseAction.action.canceled += ResumeGame;
        
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        Player.TogglePause();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame(InputAction.CallbackContext context)
    {
        Player.TogglePause();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
