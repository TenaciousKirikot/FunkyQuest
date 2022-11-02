
using System;
using UnityEngine;

namespace FunkyQuest
{    
    public class CharacterPhysics : MonoBehaviour
    {
        [SerializeField]            private Animator                _linkedAnimator;
        [SerializeField]            private SpriteRenderer          _linkedRenderer;
        [SerializeField]            private Rigidbody2D             _linkedRigidbody;
        [SerializeField]            private string                  _idlingState;
        [SerializeField]            private string                  _lookingUpState;
        [SerializeField]            private string                  _crouchingtate;
        [SerializeField]            private string                  _walkingState;
        [field: SerializeField]     public bool                     CanInput { get; set; }
        [SerializeField]            private float                   _speed;
        [SerializeField][ReadOnly]  private CharacterAnimationState _state;

        private void Update()
        {
            if (CanInput)
            {
                float vertical = Input.GetAxis("Vertical");
                if (vertical != 0)
                {
                    bool isLookingUp = vertical > 0;
                    if (isLookingUp)
                    {
                        SetState(CharacterAnimationState.LookingUp, _lookingUpState);
                        //Lookup Logic
                    }
                    else
                    {
                        SetState(CharacterAnimationState.Crouching, _crouchingtate);
                        //Crouch logic
                    }

                    _linkedRigidbody.velocity = new Vector2(0, _linkedRigidbody.velocity.y);
                }
                else
                {
                    float horizontal = Input.GetAxis("Horizontal");
                    if (horizontal != 0)
                    {
                        SetState(CharacterAnimationState.Walking, _walkingState);
                        _linkedRigidbody.velocity = new Vector2(horizontal * _speed, _linkedRigidbody.velocity.y);
                        _linkedRenderer.flipX = horizontal < 0;
                    }
                    else
                    {
                        SetState(CharacterAnimationState.Idle, _idlingState);
                        _linkedRigidbody.velocity = new Vector2(0, _linkedRigidbody.velocity.y);
                    }
                }
            }
        }

        private void SetState(CharacterAnimationState value, string state)
        {
            if (_state != value)
            {
                _state = value;
                _linkedAnimator.SetBool(_idlingState, false);
                _linkedAnimator.SetBool(_lookingUpState, false);
                _linkedAnimator.SetBool(_crouchingtate, false);
                _linkedAnimator.SetBool(_walkingState, false);
                _linkedAnimator.SetBool(state, true);
            }
        }
    }
}