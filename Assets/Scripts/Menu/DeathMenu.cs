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


        /*if (playerEntity != null)
        {
            playerEntity.OnDeath += ShowDeathMenu;
        }
        else
        {
            Debug.LogError("No player entity");
        }*/
    }
    
        public void ShowDeathMenu()
    { 
        Debug.Log("You died!");
        //Shows the menu
        deathMenu.SetActive(true);
        Debug.Log($"DeathMenu active: {deathMenu.activeSelf}");
        
         //Disaples the player
        Player.enabled = false;
        Weapon.enabled = false;


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


    public void OnMainMenuButtonClicked()
    {

        //Load the menu music list here
        //SceneManager.LoadScene("MainMenuScene");

        DataManager.Instance.ResetAllStats();
    }
}
