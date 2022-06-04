using System;
using System.Collections;
using Overtrashed.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Overtrashed.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody rb;

        private InputAction _moveAction;
        private InputAction _interactAction;
        private InputAction _pickupAction;

        private Vector3 _inputDirection;
        private Vector2 inputMovement;
        [SerializeField] float movementSpeed;
        [SerializeField] float inputDelay;

        private Transform slot;

        private bool _hasSubscribedControllerEvents;
        private bool _isActive;
        private IPickable currentPickable;

        private InputController inputController;

        private InteractableController interactableController;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            inputController = new InputController();
            _interactAction = inputController.Player1.Interact;
            _moveAction = inputController.Player1.Move;
            _pickupAction = inputController.Player1.PickUp;

            interactableController = GetComponentInChildren<InteractableController>();
            slot = transform.Find("Slot");
            
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
            _pickupAction.performed += HandlePickUp;
        }

        private void UnSubscribeControllerEvents()
        {
            if (_hasSubscribedControllerEvents == false) return;

            _hasSubscribedControllerEvents = false;
            _interactAction.performed -= HandleInteract;
            _pickupAction.performed -= HandlePickUp;
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
            interactableController.currentInteractable?.Interact(this);
        }

        private void HandlePickUp(InputAction.CallbackContext context)
        {
            var interactable = interactableController.currentInteractable;

            // Empty hand
            if (currentPickable == null)
            {
                currentPickable = interactable as IPickable;

                // Pickable item
                if (currentPickable != null)
                {
                    // Set PickUp Animation
                    // Play PickUp Sound
                    currentPickable.Pick();
                    interactableController.Remove(currentPickable as Interactable);
                    currentPickable.gameObject.transform.SetPositionAndRotation(slot.position, Quaternion.identity);
                    currentPickable.gameObject.transform.SetParent(slot);
                    return;
                }

                // Interactable ONLY ( not pickable )
                currentPickable = interactable?.TryToPickUpFromSlot(currentPickable);
                if (currentPickable != null)
                {
                    // Set PickUp Animation
                    // Play PickUp Sound
                    currentPickable.gameObject.transform.SetPositionAndRotation(slot.position, Quaternion.identity);
                    currentPickable.gameObject.transform.SetParent(slot);
                    return;
                }

            }

            // Carrying a Pickable, try to drop
            // No interactable in range or holding interactable
            if (interactable == null || interactable is IPickable)
            {
                // SetUp Drop (Back to idle) Animation
                // Play Drop Sound
                currentPickable.Drop();
                currentPickable = null;
                return;
            }
        }

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

}