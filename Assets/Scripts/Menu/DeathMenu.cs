using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
public class DeathMenu : MonoBehaviour
{

    public GameObject deathMenu;

    public Player.CharacterControls Player;
    public Player.WeaponController Weapon;

    void Start()
    {
        deathMenu.SetActive(false);
    }




    public void RestartLevel()
    {
        Player.enabled = true;
        Weapon.enabled = true;
        deathMenu.SetActive(false);
        Time.timeScale = 1f; 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void OnRestartButtonClicked()
    {
        RestartLevel();
    }
}
