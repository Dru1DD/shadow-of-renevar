using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MyGame.PlayerController
{
    [RequireComponent(typeof(CharacterController), typeof(AudioSource))]
    public class FirstPlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float lookSpeed = 2f;
        public float jumpHeight = 1.5f;
        public float gravity = -9.81f;

        private CharacterController controller;
        private Vector3 velocity;
        private Transform cameraTransform;
        private float xRotation = 0f;

        public AudioClip[] footstepSounds;
        private AudioSource audioSource;
        private float stepCooldown = 0.3f;
        private float stepTimer;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            cameraTransform = Camera.main.transform;
            audioSource = GetComponent<AudioSource>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            MovePlayer();
            RotatePlayer();
            ApplyGravity();
            HandleJump();
        }

        private void MovePlayer()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            controller.Move(move * moveSpeed * Time.deltaTime);

            if (controller.isGrounded && move.magnitude > 0)
            {
                HandleFootstep();
            }
        }

        private void RotatePlayer()
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            transform.Rotate(Vector3.up * mouseX);
        }

        private void ApplyGravity()
        {
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        private void HandleFootstep()
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                PlayFootstepSound();
                stepTimer = stepCooldown;
            }
        }

        private void PlayFootstepSound()
        {
            if (footstepSounds.Length > 0)
            {
                int index = Random.Range(0, footstepSounds.Length);
                audioSource.PlayOneShot(footstepSounds[index]);
            }
        }
    }
}
