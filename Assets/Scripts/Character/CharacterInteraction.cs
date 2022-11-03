
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using TMPro;
using ExtEvents;
using UnityEngine.Playables;

namespace FunkyQuest
{
    public class CharacterInteraction : MonoBehaviour
    {
        [field: Header("Read Only")]
        [field: SerializeField][field: ReadOnly]    public  bool                IsInteracting { get; set; }

        [Header("Properties")]
        [SerializeField]                            private TMP_Text            _interactionSign;
        [SerializeField]                            private Rect                _interactionRect;
        [SerializeField]                            private LayerMask           _interactionMask;
        [SerializeField]                            private KeyCode             _interactionKey;
        [SerializeField]                            private PlayableDirector    _director;
        [SerializeField]                            private ExtEvent            _onInteractionStart;
        [SerializeField]                            private ExtEvent            _onInteractionEnd;

        private void Update()
        {
            if (IsInteracting)
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
                    bool isHolding = Input.GetKeyDown(_interactionKey);
                    if (isHolding)
                    {
                        void OnDirectorStopped(PlayableDirector directror)
                        {
                            directror.stopped -= OnDirectorStopped;
                            _onInteractionEnd.Invoke();
                            IsInteracting = false;
                        };

                        IsInteracting = true;
                        _director.stopped += OnDirectorStopped;

                        _onInteractionStart.Invoke();
                        _director.Play();
                    }
                }
            }
        }
    }
}