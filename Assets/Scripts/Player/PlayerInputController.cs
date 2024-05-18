using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerInput _playerInput;
    private CameraController _cameraController;

    private void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();
        _cameraController = FindObjectOfType<CameraController>();

        _playerInput = new PlayerInput();
        _playerInput.PlayerMovement.Movement.performed += 
            value => _playerController.SetMovementInput(value.ReadValue<Vector2>());

        _playerInput.PlayerMovement.Camera.performed += 
            value => _cameraController.RotateCamera(value.ReadValue<Vector2>());

        _playerInput.Enable();
    }
}
