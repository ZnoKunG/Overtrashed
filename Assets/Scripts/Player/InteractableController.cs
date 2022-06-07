using System.Collections.Generic;
using JetBrains.Annotations;
using Overtrashed.Model;
using UnityEngine;

namespace Overtrashed.Player
{
    public class InteractableController : MonoBehaviour
    {
        [SerializeField] private Transform playerPivot;
        private readonly HashSet<Interactable> _interactables = new HashSet<Interactable>();

        private void Awake()
        {
            if (playerPivot == null)
            {
                playerPivot = transform;
            }
        }

        /// <summary>
        /// Get the current highlighted interactable (in range). Null if there is none in range.
        /// </summary>
        
        [CanBeNull]
        public Interactable currentInteractable { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            Interactable interactable = other.gameObject.GetComponent<Interactable>();
            if (interactable == null) return;

            if (_interactables.Contains(interactable))
            {
                Debug.LogWarning($"[InteractableController] TriggerEnter on a preexisting collider {other.gameObject.name}");
                return;
            }
            _interactables.Add(interactable);
        }
        private void OnTriggerExit(Collider other)
        {
            Interactable interactable = other.gameObject.GetComponent<Interactable>();
           if (interactable != null)
            {
                _interactables.Remove(interactable);
            }
        }

        public void Remove(Interactable interactable)
        {
            _interactables.Remove(interactable);
        }

        private void FixedUpdate()
        {
            Interactable closest = TryGetClosestInteractable();
            if (closest == currentInteractable) return;

            // Something has changed
            currentInteractable?.ToggleHighlightOff(); // check if hand is holding any interactable?
            currentInteractable = closest;

            closest?.ToggleHighlightOn(); // Also check if there is nearby interactable object
        }

        private Interactable TryGetClosestInteractable()
        {
            var minDistance = float.MaxValue;
            Interactable closest = null; // in case there is no interactable nearby
            foreach (var interactable in _interactables)
            {
                var distance = Vector3.Distance(playerPivot.position, interactable.gameObject.transform.position);
                if (distance > minDistance) continue;
                minDistance = distance;
                closest = interactable;
            }

            return closest;
        }

    }
}

