using System.Collections.Generic;
using UnityEngine;


namespace Overtrashed.Model
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Trash : Interactable, IPickable
    {
        private Rigidbody _rb;
        private Collider _collider;

        public TrashStatus trashStatus { get; private set; }
        [SerializeField] private TrashStatus startingStatus = TrashStatus.NotSorted;
        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }
        public void Pick()
        {
            _rb.isKinematic = true;
            _collider.enabled = false;
        }

        public void Drop()
        {
            gameObject.transform.SetParent(null);
            _rb.isKinematic = false;
            _collider.enabled = true;
        }

        public void ChangeToDirty()
        {
            trashStatus = TrashStatus.Dirty;
        }

        public void ChangeToWet()
        {
            trashStatus = TrashStatus.Wet;
        }

        public void ChangeToRaw()
        {
            trashStatus = TrashStatus.Raw;
        }

        public override bool TryToDropIntoSlot(IPickable pickableToDrop)
        {
            // Nothing gonna drop into the trash
            return false;
        }

        public override IPickable TryToPickUpFromSlot(IPickable playerHoldPickable)
        {
            Debug.Log($"[Ingredient] Trying to PickUp {gameObject.name}");
            _rb.isKinematic = true;
            return this;
        }
    }

}