using UnityEngine;
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

    [Command]
    public void CmdAnimate(float amt) { RpcAnimate(amt); }

    [ClientRpc]
    public void RpcAnimate(float amt) { characterAnimator.SetFloat("Speed", amt); }

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
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        characterAnimator.SetFloat("Speed", moveDirection.magnitude);
        CmdAnimate(moveDirection.magnitude);
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime * speed);
    }

    public override void OnStartLocalPlayer()
    {
        MyCamera = Instantiate(Camera.main);
        MyCamera.GetComponent<CameraController>().setTarget(gameObject);
        
    }
}