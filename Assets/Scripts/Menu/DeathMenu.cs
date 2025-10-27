using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
public class DeathMenu : MonoBehaviour
{

    public GameObject deathMenu;
    public Entity playerEntity;
    public Player.CharacterControls Player;
    public Player.WeaponController Weapon;

    void Start()
    {
        deathMenu.SetActive(false);


        if (playerEntity != null)
        {
            playerEntity.OnDeath += ShowDeathMenu;
        }
        else
        {
            Debug.LogError("No player entity");
        }
    }
    
        public void ShowDeathMenu()
    {   //Disaples the player
        Player.enabled = false;
        Weapon.enabled = false;

        //Shows the menu
        deathMenu.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }




    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    
    public void OnRestartButtonClicked()
    {
        RestartLevel();
    }
}
