using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

namespace DereDere.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6.0f;

        private float horizontal;
        private float vertical;
        private float rotationSpeed = 700.0f;
        private float gravity = -19.62f;

        private CharacterController characterController;

        private Transform playerCamera;

        private Vector3 inputDirection;
        private Vector3 moveDirection;
        private Vector3 velocity;

        private void Start()
        {
            // Get the relevant references
            characterController = GetComponent<CharacterController>();
            playerCamera = Camera.main.transform;
        }

        private void Update()
        {
            GetMovementInput();
            ResetVelocity();
            HandleMovement();
            HandleGravity();
        }

        private void GetMovementInput()
        {
            // Get the movement input in both the x and y axis from the input class
            horizontal = PlayerInput.Instance.MovementInput().x;
            vertical = PlayerInput.Instance.MovementInput().y;
        }

        private void HandleMovement()
        {
            // Get the input direction vector
            inputDirection = new Vector3(horizontal, 0.0f, vertical).normalized;
            // Reset the move direction vector
            moveDirection = Vector3.zero;

            if (inputDirection.magnitude >= 0.1f)
            {
                // Calculate the rotation angle relative to the camera
                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float currentAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
                // Update the rotation of the player using the current angle
                transform.rotation = Quaternion.Euler(0.0f, currentAngle, 0.0f);

                // Calculate the move direction 
                moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
                // Move the character controller
                characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            }

        }

        private void HandleGravity()
        {
            // Calcilate the y velocity
            velocity.y += gravity * Time.deltaTime;
            // Apply the gravity by moving the character controller
            characterController.Move(velocity * Time.deltaTime);
        }

        private void ResetVelocity()
        {
            if (IsGrounded() && velocity.y < 0.0f)
            {
                // Reset the velocity to make character stick to the ground
                velocity.y = -2.0f;
            }
        }

        public bool IsGrounded()
        {
            // Public return for the character controller grounded
            return characterController.isGrounded;
        }
    }
}
