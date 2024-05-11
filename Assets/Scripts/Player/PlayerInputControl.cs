using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControl : MonoBehaviour
{
    private PlayerInput _playerInput;

    private void OnEnable()
    {
        if( _playerInput == null )
        {
            _playerInput = new PlayerInput();

            _playerInput.Player.Jump.performed += OnJumpPressed;
            _playerInput.Player.Movement.performed += OnMovement;
        }
        _playerInput.Enable();
    }

    void OnJumpPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump pressed");
    }

    void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }
}
