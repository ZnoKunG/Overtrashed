using System.Collections;
using System.Collections.Generic;
using Overtrashed.Model;
using Overtrashed.Player;
using UnityEngine;

namespace Overtrashed.Appliances
{
    public class Countertop : Interactable
    {
        public override bool TryToDropIntoSlot(IPickable pickableToDrop)
        {
            if (CurrentPickable == null) return TryDropIfNotOccupied(pickableToDrop);

            return CurrentPickable switch
            {
                Trash trash => trash.TryToDropIntoSlot(pickableToDrop),
                _ => false
            };
        }

        public override IPickable TryToPickUpFromSlot(IPickable playerHoldPickable)
        {
            if (CurrentPickable == null) return null; // No trash inside this countertop

            var output = CurrentPickable;
            var interactable = CurrentPickable as Interactable;
            interactable?.ToggleHighlightOff();
            CurrentPickable = null;
            return output;
        }

        // IN CASE : There is no trash inside this countertop
        private bool TryDropIfNotOccupied(IPickable pickable)
        {
            if (CurrentPickable != null) return false;

            CurrentPickable = pickable;
            CurrentPickable.gameObject.transform.SetParent(Slot);
            CurrentPickable.gameObject.transform.SetPositionAndRotation(Slot.position, Quaternion.identity);
            return true;
        }
    }
}
