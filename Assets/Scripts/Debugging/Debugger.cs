
using System;
using System.Reflection;
using UnityEngine;

namespace FunkyQuest
{
    internal class Debugger : MonoBehaviour
    {
        [SerializeField][ReadOnly]  private CharacterInteraction[]  _interactions;
        [SerializeField]            private Color[]                 _interactionsColors;
                                    private FieldInfo               _interactionRectFieldInfo;

        private void OnValidate()
        {
            _interactions = transform.GetComponentsInChildren<CharacterInteraction>();
            int newLength = _interactions.Length;
            int oldLength = _interactionsColors.Length;

            Array.Resize(ref _interactionsColors, newLength);
            for (int i = oldLength; i < newLength && newLength > oldLength; i++)
            {
                _interactionsColors[i] = new Color(0, 255, 0, 0.35f);
            }

            Type characterInteraction = typeof(CharacterInteraction);
            _interactionRectFieldInfo = characterInteraction.GetField("_interactionRect", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private void OnDrawGizmos()
        {
            Color gismozColor = Gizmos.color;

            for (int i = 0; i < _interactions.Length; i++)
            {
                CharacterInteraction interaction = _interactions[i];
                Vector2 position = interaction.transform.position;
                Rect interactionRect = (Rect)_interactionRectFieldInfo.GetValue(interaction);

                Gizmos.color = _interactionsColors[i];
                Gizmos.DrawCube(position + interactionRect.position, interactionRect.size);
            }

            Gizmos.color = gismozColor;
        }
    }
}