using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DereDere.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance = null;

        public float mouseSensitivity = 2.0f;

        public event EventHandler<EventArgs> OnInteractPressed;

        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            // Set up a singleton for this input manager
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                // Remove and update the singleton
                Destroy(Instance);
                Instance = this;
            }

            playerInputActions = new PlayerInputActions();
        }

        private void Update()
        {
            ButtonInput();
        }

        public Vector2 MovementInput()
        {
            // Get the movement input from input actions as a 2-dimensional vector
            return playerInputActions.Player.Movement.ReadValue<Vector2>();
        }

        public Vector2 CameraInput()
        {
            // Get the camera look input from input actions as a 2-dimensional vector
            return playerInputActions.Player.CameraLook.ReadValue<Vector2>();
        }

        public float MouseSensitivity()
        {
            // Get the mouse sensitivity input from input actions as a float
            return (mouseSensitivity * 100.0f);
        }

        public void ButtonInput()
        {
            // Using c# events for button inputs, may need to be adapted for hold inputs
            playerInputActions.Player.Interact.performed += ctx => OnInteractPressed?.Invoke(this, EventArgs.Empty);
        }

        private void OnEnable()
        {
            playerInputActions.Player.Enable();
        }

        private void OnDisable()
        {
            playerInputActions.Player.Disable();
        }
    }
}
