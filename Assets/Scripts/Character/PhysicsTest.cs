using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPlatformer
{
    public class PhysicsTest : MonoBehaviour
    {
        private Rigidbody2D _rb;

        [SerializeField] private float _force;

        [SerializeField] private float _torque;

        private void Awake()
        {
            TryGetComponent(out _rb);
        }

        // Start is called before the first frame update
        void Start()
        {
            //Push();
            //Rotate();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void Push()
        {
            _rb.AddForce(new Vector2(_force, 0f), ForceMode2D.Impulse);
        }

        void Rotate()
        {
            _rb.AddTorque(_torque, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log($"trigger enter: {collider.gameObject.name}");
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            Debug.Log($"trigger stay: {collider.gameObject.name}");
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            Debug.Log($"trigger exit: {collider.gameObject.name}");
        }
    }
}
