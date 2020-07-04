using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 movement;

    public JoystickController joystick;
    public float moveSpeed, jumpSpeed = 10f, gravity = 10f;

    private void Start()
    {
        // Get CharacterController Component
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float moveHorizontal = joystick.Direction.x;
        float moveVertical = joystick.Direction.z;
        
        if (characterController.isGrounded)
        {
            movement = new Vector3(moveVertical, 0f, -moveHorizontal);
            movement *= moveSpeed;
        }
        
        movement.y -= gravity * Time.deltaTime;
        
        characterController.Move(movement * Time.deltaTime);
    }

    public void Jump()
    {
        
    }
}
