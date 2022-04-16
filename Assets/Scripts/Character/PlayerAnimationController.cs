using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPlatformer
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private PlayerCharacter _character;
        [SerializeField] private CharacterCollision _characterCollision;
        [SerializeField] private Rigidbody2D _rb;

        [SerializeField] private string _idleState;
        [SerializeField] private string _runState;
        [SerializeField] private string _airState;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            ChangeFacingDirection();

            //_animator.SetBool("Moving", _character.horizontalIntent != 0f);
            //_animator.SetBool("InAir", !_characterCollision.onGround);
            _animator.SetFloat("VelocityY", _rb.velocity.y);

            if (!_characterCollision.onGround)
            {
                PlayAnimIfNotPlaying(_airState);
            }
            else
            {
                if (_character.horizontalIntent != 0f)
                {
                    PlayAnimIfNotPlaying(_runState);
                }
                else
                {
                    PlayAnimIfNotPlaying(_idleState);
                }
            }
        }

        void ChangeFacingDirection()
        {
            if (_character.horizontalIntent > 0f)
            {
                _character.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (_character.horizontalIntent < 0f)
            {
                _character.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        void PlayAnimIfNotPlaying(string stateName)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) return;
            _animator.Play(stateName);
        }

        public void AE_PlayFootstepSfx()
        {
            //Debug.Log("play footstep sfx");
        }
    }
}
