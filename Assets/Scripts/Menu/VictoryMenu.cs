using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public Entity playerEntity;
    public GameObject victoryMenu;
    public Player.CharacterControls Player;
    public Player.WeaponController Weapon;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        victoryMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void GameWon()
    {
        Player.enabled = false;
        Weapon.enabled = false;
        victoryMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log(victoryMenu.activeSelf);
    }
}
