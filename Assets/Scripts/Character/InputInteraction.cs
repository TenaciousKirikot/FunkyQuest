
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
    public class InputInteraction: MonoBehaviour
    {
        [Header("Read Only")]
        [SerializeField][ReadOnly]  private bool                _hasAccepted;

        [Header("Properties")]
        [SerializeField]            private string              _acceptedString;
        [SerializeField]            private TMP_InputField      _inputField;
        [SerializeField]            private PlayableDirector    _director;
        [SerializeField]            private ExtEvent            _acceptedStart;
        [SerializeField]            private ExtEvent            _acceptedEnd;
            
        private void Start()
        {
            _inputField.onValueChanged.AddListener(
                s =>
                {
                    if (s.ToLower() == _acceptedString.ToLower() && !_hasAccepted)
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
            );
        }
    }
}