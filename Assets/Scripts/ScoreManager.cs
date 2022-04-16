using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyPlatformer
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        private int _score;

        [SerializeField] private UnityEvent<int> onScoreUpdated;

        public void AddScore(int increment)
        {
            _score += increment;
            onScoreUpdated?.Invoke(_score);
        }
    }
}
