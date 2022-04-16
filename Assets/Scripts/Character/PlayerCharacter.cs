using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace MyPlatformer
{
    [SelectionBase]
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Walk")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _acceleration = 5f;

        [Header("Jump")]
        [SerializeField] private float _jumpHeight = 1f;
        [SerializeField] private int _maxNumberOfJump = 1;

        [Header("Jump buffer")]
        [SerializeField] private float _jumpBufferTime = 0.2f;

        [Header("Coyote time")]
        [SerializeField] private float _coyoteTime = 0.1f;

        [Header("Jump cancel")]
        [Range(0f, 1f)]
        [SerializeField] private float _cancelJumpMult;
        [SerializeField] private float _jumpMinTime = 0.1f;


        [SerializeField] private CharacterCollision _characterCollision;

        private PlayerControls _controls;
        private float _horizontalIntent;
        public float horizontalIntent => _horizontalIntent;

        private Rigidbody2D _rb;

        private bool _jumpQueued;
        private float _jumpQueueTimestamp = 0f;
        private bool _isJumping;
        private int _numberOfJumpsLeft = 0;
        private float _notGroundedTimer = 0f;
        private bool _jumpCancelQueued;
        private bool _jumpCanceled;
        private float _jumpTimer;

        private void Awake()
        {
            _controls = new PlayerControls();

            TryGetComponent(out _rb);
        }

        private void OnEnable()
        {
            _controls.Main.Enable();
            _controls.Main.Jump.performed += HandleJump;
            _controls.Main.Jump.canceled += HandleJumpCancel;
        }

        private void OnDisable()
        {
            _controls.Main.Jump.performed -= HandleJump;
            _controls.Main.Jump.canceled -= HandleJumpCancel;
            _controls.Main.Disable();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void FixedUpdate()
        {
            CheckGround();

            if (_isJumping)
            {
                _jumpTimer += Time.deltaTime;
            }

            CheckJumpCancel();

            _horizontalIntent = _controls.Main.Move.ReadValue<Vector2>().x;
            if (_horizontalIntent != 0f)
            {
                _horizontalIntent /= Mathf.Abs(_horizontalIntent);
            }

            Walk();

            CheckJumpQueued();
        }

        private void Walk()
        {
            if (!CanWalk()) return;

            Vector2 targetVelocity = new Vector2(_speed * _horizontalIntent, _rb.velocity.y);

            _rb.velocity = Vector2.MoveTowards(_rb.velocity, targetVelocity, _acceleration);
            if (_characterCollision.onGround && !_isJumping)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Min(0f, _rb.velocity.y));
            }
        }

        private bool CanWalk()
        {
            return true;
        }

        private void HandleJump(InputAction.CallbackContext context)
        {
            _jumpQueued = true;
            _jumpQueueTimestamp = Time.time;
            _jumpCancelQueued = false;
        }

        private void HandleJumpCancel(InputAction.CallbackContext context)
        {
            _jumpCancelQueued = true;
        }

        private void CheckGround()
        {
            if (_rb.velocity.y <= 0f)
            {
                _isJumping = false;
            }

            if (_characterCollision.onGround)
            {
                if (!_isJumping)
                {
                    _numberOfJumpsLeft = _maxNumberOfJump;
                }

                _notGroundedTimer = 0f;
            }
            else
            {
                _notGroundedTimer += Time.deltaTime;
                if (_notGroundedTimer > _coyoteTime && _numberOfJumpsLeft == _maxNumberOfJump)
                {
                    _numberOfJumpsLeft--;
                }
            }
        }

        private void CheckJumpQueued()
        {
            if (!_jumpQueued) return;

            // check input buffer
            if (Time.time - _jumpQueueTimestamp <= _jumpBufferTime)
            {
                if (CanJump())
                {
                    Jump();
                }
            }
            else
            {
                _jumpQueued = false;
            }
        }

        private bool CanJump()
        {
            return _numberOfJumpsLeft > 0;
        }

        private void Jump()
        {
            _jumpQueued = false;
            _isJumping = true;
            _numberOfJumpsLeft--;
            float jumpSpeed = Mathf.Sqrt(2 * GetGravityMagnitude() * _jumpHeight);
            _rb.velocity = new Vector2(_rb.velocity.x, jumpSpeed);
            _jumpTimer = 0f;
            _jumpCanceled = false;
        }

        private void CheckJumpCancel()
        {
            if (_jumpCancelQueued && _jumpTimer >= _jumpMinTime && !_jumpCanceled)
            {
                _jumpCanceled = true;
                if (_rb.velocity.y > 0f && _isJumping)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _cancelJumpMult);
                }
            }
        }

        private float GetGravityMagnitude()
        {
            return Mathf.Abs(Physics2D.gravity.y * _rb.gravityScale);
        }
    }
}
