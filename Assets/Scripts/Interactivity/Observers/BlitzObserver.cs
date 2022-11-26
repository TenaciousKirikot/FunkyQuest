
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace FunkyQuest
{
    internal class BlitzObserver : ConditionObserver
    {
        [field: Header("Text Input Observer - Read Only")]
        [field: SerializeField][field: ReadOnly]    public int          QuestionIndex { get; set; }
        [SerializeField][ReadOnly]                  private float       _currentTime;

        [Header("Blitz Observer - Properties")]
        [SerializeField]                            private Button[]    _options;
        [SerializeField]                            private TMP_Text[]  _labels;
        [SerializeField]                            private int         _count;
        [SerializeField]                            private string[]    _text;
        [SerializeField]                            private int[]       _answers;
        [SerializeField]                            private float       _time;
        [SerializeField]                            private UnityEvent  _failed;

        private void Start()
        {
            for (int i = 0; i < _options.Length; i++)
            {
                Button option = _options[i];
                option.onClick.AddListener(
                    () =>
                    {
                        if (option == _options[_answers[QuestionIndex]])
                        {
                            if (++QuestionIndex == _count)
                            {
                                IsFulfilled = true;
                            }
                            else
                            {
                                UpdateText();
                            }
                        }
                        else
                        {
                            _failed.Invoke();
                        }
                    }
                );
            }

            UpdateText();
        }

        private void FixedUpdate()
        {
            if (!IsFulfilled)
            {
                _currentTime -= Time.fixedDeltaTime;

                if (_currentTime <= 0)
                {
                    _failed.Invoke();
                }
            }
        }

        private void UpdateText()
        {
            for (int i = 0; i < _labels.Length; i++)
            {
                _labels[i].text = _text[QuestionIndex * _labels.Length + i];
            }

            _currentTime = _time;
        }
    }
}