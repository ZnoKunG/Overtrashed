
using System.Collections.Generic;
using UnityEngine;

namespace Overtrashed.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public string levelName;
        [Tooltip("Trashes that going to be randomly spawned")]
        public List<TrashData> trashOrders;
        [Tooltip("Duration of the level (in seconds)")]
        public int durationTime;
        [Header("Star Rating")]
        public int star1Score;
        public int star2Score;
        public int star3Score;
    }

}