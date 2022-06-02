using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private InputAction _moveAction;
    private InputAction _interactAction;

    private Vector3 _inputDirection;
    private Vector2 inputMovement;
    [SerializeField] float movementSpeed;
    [SerializeField] float inputDelay;

    private bool _hasSubscribedControllerEvents;
    private bool _isActive;

    private InputController inputController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputController = new InputController();
        _interactAction = inputController.Player1.Interact;
        _moveAction = inputController.Player1.Move;
    }

    private void OnEnable()
    {
        inputController.Player1.Enable();
        SubscribeControllerEvents();
        SubscribeInteractableEvents();
    }

    private void OnDisable()
    {
        UnSubscribeControllerEvents();
        UnSubscribeInteractableEvents();   
    }
    private void SubscribeControllerEvents()
    {
        if (_hasSubscribedControllerEvents) return;

        _hasSubscribedControllerEvents = true;
        //_moveAction.performed += HandleMove;
        _interactAction.performed += HandleInteract;
    }

    private void UnSubscribeControllerEvents()
    {

    }

    private void SubscribeInteractableEvents()
    {

    }

    private void UnSubscribeInteractableEvents()
    {

    }

    private void Update()
    {
        CalculateInputDirection();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        TurnPlayer();
        AnimatePlayerMovement();
    }

    private void MovePlayer()
    {
        rb.AddForce(_inputDirection * movementSpeed, ForceMode.Force);
    }
    private void HandleInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
    }

    /*private void HandleMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        OptimizeMoveInput();
        _inputDirection = new Vector3(inputMovement.x, 0f, inputMovement.y);
        Debug.Log("Handle Move : " + _inputDirection);
    }*/

    private void CalculateInputDirection()
    {
        inputMovement = _moveAction.ReadValue<Vector2>();
        OptimizeMoveInput();
        Debug.Log("CalculateInputDirection : " + _inputDirection);
    }

    private void OptimizeMoveInput()
    {

        if (inputMovement.x > inputDelay)
        {
            inputMovement.x = 1.0f;
        }
        else if (inputMovement.x < -inputDelay)
        {
            inputMovement.x = -1.0f;
        }
        else
        {
            inputMovement.x = 0f;
        }

        if (inputMovement.y > inputDelay)
        {
            inputMovement.y = 1.0f;
        }
        else if (inputMovement.y < -inputDelay)
        {
            inputMovement.y = -1.0f;
        }
        else
        {
            inputMovement.y = 0f;
        }
        _inputDirection = new Vector3(inputMovement.x, 0f, inputMovement.y);
    }
    private void TurnPlayer()
    {
        if (!(rb.velocity.magnitude > 0.1f) || _inputDirection == Vector3.zero) return;

        Quaternion newRotation = Quaternion.LookRotation(_inputDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 15f);

    }

    private void AnimatePlayerMovement()
    {
        // Animate PlayerMovement
    }
}
