using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPlatformer
{
    public class ScorePickup : MonoBehaviour
    {
        [SerializeField] private int _scoreAmount = 1;

        private PlayerCharacter _player;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out _player))
            {
                ScoreManager.instance.AddScore(_scoreAmount);
                Destroy(gameObject);
            }
        }
    }
}
