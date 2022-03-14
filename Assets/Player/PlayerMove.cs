/// <summary>
/// Handle functions to move the player in the environment.
///
/// Adapted From: https://github.com/Hero3D/MouseLook
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handle functions to move the player in the environment
/// </summary>
public class PlayerMove : MonoBehaviour {

    /// <summary>
    /// Movement direciton
    /// </summary>
    private Vector3 _moveDirection;

    /// <summary>
    /// CaracterController component
    /// </summary>
    private CharacterController _controller;

    /// <summary>
    /// Walking Speed
    /// </summary>
    public float speed = 5.0f;

    /// <summary>
    /// Jump Force
    /// </summary>
    public float jumpForce = 13.0f;

    /// <summary>
    /// Anti Bump factor
    /// </summary>
    public float antiBump = 4.5f;

    // Gravity Force
    public float gravity = 30.0f;

    /// <summary>
    /// Initialize game controller
    /// </summary>
    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Calls the movement controller
    /// </summary>
    private void Update() {
        DefaultMovement();
    }

    /// <summary>
    /// Performe the move function in the fixed update since it is a physics issue
    /// </summary>
    private void FixedUpdate() {
        _controller.Move(_moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Control movement and jumping behaviours
    /// </summary>
    private void DefaultMovement() {
        if (_controller.isGrounded) {

            // Get Walinkg input direction
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Normalize the walking
            if (input.x !=0 && input.y !=0) {
                input *= 0.777f;
            }

            // Move x and y
            _moveDirection.x = input.x * speed;
            _moveDirection.z = input.y * speed;
            _moveDirection.y = -antiBump;

            _moveDirection = transform.TransformDirection(_moveDirection);

            // Read key to jump
            if(Input.GetKey(KeyCode.Space)){
                Jump();
            }
        } else {
            // Perform gravity forces if on air
            _moveDirection.y -= gravity * Time.deltaTime;
        }

    }

    /// <summary>
    /// Handle Jump forcecs
    /// </summary>
    private void Jump() {
        _moveDirection.y += jumpForce;
    }
}
