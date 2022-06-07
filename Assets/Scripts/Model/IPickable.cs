using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtrashed.Model
{
    public interface IPickable
    {
        GameObject gameObject { get; }
        void Pick();
        void Drop();
    }
}
