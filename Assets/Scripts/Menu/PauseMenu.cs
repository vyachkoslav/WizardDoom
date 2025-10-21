using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;



public class PauseMenu : MonoBehaviour
{


    public GameObject pauseMenu;
    public Player.CharacterControls Player;
    public Player.WeaponController Weapon;
    [SerializeField] private InputActionReference pauseAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void OnEnable()
    {
        pauseAction.action.performed += TogglePause;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        pauseAction.action.performed -= TogglePause;
        
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
        Player.enabled = false;
        Weapon.enabled = false;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log(pauseMenu.activeSelf);
    }

    public void ResumeGame()
    {
        Player.enabled = true;
        Weapon.enabled = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }



    public void OnResumeButtonCliked()
    {
        Debug.Log("Button Pressed");
        ResumeGame();
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}


