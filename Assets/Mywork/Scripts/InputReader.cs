using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.Timeline.TimelinePlaybackControls;

namespace TheControl
{

    public class InputReader : MonoBehaviour
    {
        private Vector2 move;
        private Inputmanager _Inputmanager;
        private PlayerController player;
        private void OnEnable()
        {
            _Inputmanager = new Inputmanager();

            _Inputmanager.Player.Move.performed += value => player.Straf(value.ReadValue<Vector2>());



            _Inputmanager.Enable();                    
            
          
        }
      

        public event Action<Vector2> MoveEvent;

        public event Action JumpEvent;
        public event Action FireEvent;

        public event Action <DeltaControl> LookEvent;

        public void OnFire(InputAction.CallbackContext context)
        {
           
            
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                JumpEvent?.Invoke();
            }
          

        }

        public void OnLook(InputAction.CallbackContext context)
        {
            

        }
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }
}