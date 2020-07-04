using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 movement;

    public JoystickController joystick;
    public float moveSpeed = 5f, jumpSpeed = 10f, gravity = 10f;

    private void Start()
    {
        // Get CharacterController Component
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
    }

    /// <summary>
    /// Movement: to move player with joystick
    /// </summary>
    private void Movement()
    {
        // Get movement from joystick
        float moveHorizontal = joystick.Direction.x;
        float moveVertical = joystick.Direction.z;
        
        // Check if player is on ground
        if (characterController.isGrounded)
        {
            // Player Movement
            movement = new Vector3(moveVertical, 0f, -moveHorizontal);
            movement *= moveSpeed;
        }
        
        movement.y -= gravity * Time.deltaTime; // Add gravity
        
        characterController.Move(movement * Time.deltaTime); // Move Player
    }

    public void Jump()
    {
        
    }
}
