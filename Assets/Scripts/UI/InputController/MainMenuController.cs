using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public PlayerInputs playerControls;
    private InputAction confirm;


    private void Awake()
    {
        playerControls = new PlayerInputs();
    }

    private void OnEnable()
    {
        confirm = playerControls.Player.Confirm;
        confirm.performed += PlayGame;
        confirm.Enable();
    }

    private void OnDisable()
    {
        confirm.Disable();
    }
    
    void PlayGame(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene("SensitivityAdjusting", LoadSceneMode.Single);
    }
}
