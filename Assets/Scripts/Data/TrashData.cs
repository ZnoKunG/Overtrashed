using System.Collections.Generic;
using UnityEngine;

namespace Overtrashed.Data
{
    [CreateAssetMenu(fileName = "TrashData", menuName = "TrashData", order = 2)]
    public class TrashData : ScriptableObject
    {
        [Tooltip("UI Usage")]
        public Sprite sprite;
        [Tooltip("Components of trash")]
        public List<RawMaterialData> components;
    }
}
