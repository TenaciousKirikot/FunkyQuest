
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using TMPro;
using ExtEvents;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.Linq;

namespace FunkyQuest
{
    public class InputInteraction_Old : MonoBehaviour
    {
        [Header("Read Only")]
        [SerializeField][ReadOnly]  private string              _currentString;
        [SerializeField][ReadOnly]  private bool                _hasAccepted;

        [Header("Properties")]
        [SerializeField]            private string              _acceptedString;
        [SerializeField]            private TMP_Text            _text;
        [SerializeField]            private PlayableDirector    _director;
        [SerializeField]            private ExtEvent            _acceptedStart;
        [SerializeField]            private ExtEvent            _acceptedEnd;

                                    private IEnumerable<KeyCode> _acceptedKeys;
            
        private void Start()
        {
            _acceptedKeys = typeof(KeyCode).GetEnumValues().Cast<KeyCode>().Where(k => k > KeyCode.BackQuote && k < KeyCode.LeftCurlyBracket);
        }

        private void Update()
        {
            if (Input.anyKeyDown && !_hasAccepted)
            {
                IEnumerable<KeyCode> keys = _acceptedKeys.Where(p => Input.GetKeyDown(p));
                foreach (KeyCode key in keys)
                {
                    _currentString += key.ToString();
                }

                bool backspace = Input.GetKeyDown(KeyCode.Backspace) && _currentString.Count() > 0;
                if (backspace)
                {
                    _currentString = _currentString.Remove(_currentString.Length - 1);
                }

                if (keys.Count() > 0 || backspace)
                {
                    _text.text = _currentString;

                    if (_currentString == _acceptedString)
                    {
                        void OnDirectorStopped(PlayableDirector directror)
                        {
                            directror.stopped -= OnDirectorStopped;
                            _acceptedEnd.Invoke();
                            _hasAccepted = false;
                        };

                        _hasAccepted = true;
                        _director.stopped += OnDirectorStopped;

                        _acceptedStart.Invoke();
                        _director.Play();
                    }
                }
            }

            /*if (IsInteracting)
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
            }*/
        }
    }
}