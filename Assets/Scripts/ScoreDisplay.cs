using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyPlatformer
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void UpdateScore(int currentScore)
        {
            _scoreText.text = $"{currentScore}";
        }
    }
}
