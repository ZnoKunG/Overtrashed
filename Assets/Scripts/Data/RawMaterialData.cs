using System.Collections.Generic;
using UnityEngine;
using Overtrashed.Model;

namespace Overtrashed.Data
{
    [CreateAssetMenu(fileName = "RawMaterialData", menuName = "RawMaterialData", order = 0)]
    public class RawMaterialData : ScriptableObject
    {
        public TrashType type;
        public float washTime;
        public float dryTime;

        [Header("Visual")]
        public Mesh rawMesh;
        public Mesh DirtyMesh;
        public Mesh WetMesh;
        [Tooltip("")]
        public Material material;
        [Tooltip("")]
        public Color baseColor;
        [Tooltip("UI Usage")]
        public Sprite sprite;
        
    }
}
