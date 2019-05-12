﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class PlayerMovement : NetworkBehaviour
{
    CharacterController characterController;
    public Animator characterAnimator;
    public bool isEnabled = false;
    public NetworkInstanceId playerID;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera MyCamera;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        characterController = GetComponent<CharacterController>();
        playerID = GetComponent<NetworkIdentity>().netId;
    }


    void Update()
    {
        if (!isLocalPlayer || !isEnabled)
        {
            return;
        }
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        if(Mathf.Sqrt(Mathf.Pow(moveDirection.x,2)) > 0 || Mathf.Sqrt(Mathf.Pow(moveDirection.z, 2)) > 0)
        {
            characterAnimator.SetBool("Moving", true);
        }
        else
        {
            characterAnimator.SetBool("Moving", false);
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public override void OnStartLocalPlayer()
    {
        MyCamera = Instantiate(Camera.main);
        MyCamera.GetComponent<CameraController>().setTarget(gameObject);
        
    }
}