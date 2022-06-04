using System.Collections.Generic;
using JetBrains.Annotations;
using Overtrashed.Player;
using UnityEngine;

namespace Overtrashed.Model
{
    /// <summary>
    /// Structures used for partially highlighting a interactable object.
    /// Also highlight the object that interact to this interactable object.
    /// Allow ONLY certain items to be picked and dropped into it.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class Interactable : MonoBehaviour
    {
        [Tooltip("Pivot where IPickables could be pickedUp/dropped")]
        [SerializeField] private Transform slot;
        protected IPickable CurrentPickable { get; set; }
        protected PlayerController LastPlayerControllerInteracting;
        private readonly List<MeshRenderer> _meshes = new List<MeshRenderer>();
        private MaterialPropertyBlock _materialBlock;
        private readonly int Highlight = Shader.PropertyToID("_Highlight"); // Access "Highlight" Shader Effect with _Highlight reference
        public Transform Slot => slot;

        protected virtual void Awake()
        {
            _materialBlock = new MaterialPropertyBlock();

            CacheMeshRenderers();
            CheckSlotOccupied();
        }

        private void CacheMeshRenderers()
        {
            var baseMesh = transform.GetComponent<MeshRenderer>();
            if (baseMesh != null) _meshes.Add(baseMesh);
        }

        // Recursively access each child in the baseMesh (Except the Slot)
        private void CacheMeshRenderersRecursivelyIgnoring(Transform root)
        {
            foreach (Transform child in root)
            {
                if (child.gameObject.name == "Slot") continue;

                var meshRenderer = child.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    _meshes.Add(meshRenderer);
                }

                CacheMeshRenderersRecursivelyIgnoring(child);
            }
        }

        // Check if there is an pickable item in the slot
        private void CheckSlotOccupied()
        {
            if (Slot == null) return;
            foreach (Transform child in Slot)
            {
                CurrentPickable = child.GetComponent<IPickable>();
                if (CurrentPickable != null) return;
            }
        }

        private void ChangePropertyBlock(bool highlight)
        {
            _materialBlock?.SetInt(Highlight, highlight ? 1 : 0);

            foreach (var mesh in _meshes)
            {
                mesh?.SetPropertyBlock(_materialBlock);
            }
        }

        public virtual void Interact(PlayerController playerController)
        {
            LastPlayerControllerInteracting = playerController;
        }

        public virtual void ToggleHighlightOn()
        {
            ChangePropertyBlock(true);
            var interactable = CurrentPickable as Interactable;
            interactable?.ToggleHighlightOn();
        }
        public virtual void ToggleHighlightOff()
        {
            ChangePropertyBlock(true);
            var interactable = CurrentPickable as Interactable;
            interactable?.ToggleHighlightOff();
        }

        public abstract bool TryToDropIntoSlot(IPickable pickableToDrop);

        [CanBeNull] public abstract IPickable TryToPickUpFromSlot(IPickable playerHoldPickable);
    }

}