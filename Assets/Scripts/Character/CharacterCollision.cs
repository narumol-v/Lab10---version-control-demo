using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPlatformer
{
    public class CharacterCollision : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayers;
        [SerializeField] private float _groundCheckRadius;

        private List<Collider2D> _groundColliders = new List<Collider2D>();

        private ContactFilter2D _groundContactFilter;

        private bool _onGround;
        public bool onGround => _onGround;

        private void Awake()
        {
            _groundContactFilter = new ContactFilter2D();
            _groundContactFilter.SetLayerMask(_groundLayers);
        }

        private void FixedUpdate()
        {
            _onGround = false;
            if (Physics2D.OverlapCircle(transform.position, _groundCheckRadius, _groundContactFilter, _groundColliders) > 0)
            {
                _onGround = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, _groundCheckRadius);
        }
    }
}
