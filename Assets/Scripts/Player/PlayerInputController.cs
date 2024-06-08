using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerInput _playerInput;  // PlayerInput class is auto generated by the Input Action Asset
    private CameraController _cameraController;

    public float speed = 5f; // Speed at which the GameObject moves

    private void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();
        _cameraController = FindObjectOfType<CameraController>(); // This is an expensive method, should not be used frequently

        _playerInput = new PlayerInput();
        _playerInput.PlayerMovement.Movement.performed += 
            value => _playerController.SetMovementInput(value.ReadValue<Vector2>());

        _playerInput.PlayerMovement.Camera.performed +=
            value => _cameraController.RotateCamera(value.ReadValue<Vector2>());

        _playerInput.PlayerMovement.Jump.performed += value => _playerController.Jump();
        
        _playerInput.Enable();
    }



  
}
