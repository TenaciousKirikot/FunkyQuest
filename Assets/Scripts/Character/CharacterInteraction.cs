
using System;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

namespace FunkyQuest
{
    public class CharacterInteraction : MonoBehaviour
    {
        [Header("Read Only")]
        [SerializeField][ReadOnly]  private CharacterPhysics    _linkedPhysics;
        [SerializeField][ReadOnly]  private bool                _isInteracting;

        [Header("Properties")]
        [SerializeField]            private bool                _disableInput;
        [SerializeField]            private TMP_Text            _interactionSign;
        [SerializeField]            private Rect                _interactionRect;
        [SerializeField]            private LayerMask           _interactionMask;
        [SerializeField]            private KeyCode             _interactionKey;

        private void Start()
        {
            _linkedPhysics = GetComponentInParent<CharacterPhysics>();
        }

        private void Update()
        {
            if (_isInteracting)
            {
                _interactionSign.gameObject.SetActive(false);
            }
            else
            {
                Vector2 position = transform.position;
                RaycastHit2D raycast = Physics2D.BoxCast(position + _interactionRect.position, _interactionRect.size, 0, Vector2.zero, 0, _interactionMask);

                bool isNearInteractable = raycast.collider != null;
                _interactionSign.gameObject.SetActive(isNearInteractable);
                _interactionSign.SetText(_interactionKey.ToString());

                if (isNearInteractable)
                {
                    bool isHolding = Input.GetKey(_interactionKey);
                    if (isHolding)
                    {
                        _isInteracting = true;

                        if (_disableInput)
                        {
                            _linkedPhysics.CanInput = false;
                        }
                    }
                }
            }
        }
    }
}