using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(_playerInput == null)
        {
            _playerInput = new PlayerInput();
            _playerInput.Player.Jump.performed += OnJumpPressed;
            _playerInput.Player.Movement.performed += OnMovement;
        }
        _playerInput.Enable();
    }

    void OnJumpPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Pressed!");
    }

    void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
