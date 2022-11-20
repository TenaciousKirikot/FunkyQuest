
using System.Collections.Generic;
using UnityEngine;

namespace FunkyQuest
{
    [RequireComponent(typeof(Collider2D))]
    internal class CharacterInteractivity : MonoBehaviour
    {
        [Header("Character Interactivity - Read Only")]
        [SerializeField][ReadOnly]  private List<Interactable>      _interactables;
                                    private Dictionary<int, int>    _typeIds;

        [Header("Character Interactivity - Properties")]
        [SerializeField]            private LayerMask           _interactableLayer;

        private void OnTriggerEnter2D(Collider2D collision) => Utility.OnTriggerEvent(collision, _interactableLayer, _interactables);
        private void OnTriggerExit2D(Collider2D collision)  => Utility.OnTriggerEvent(collision, _interactableLayer, _interactables);

        private void Update()
        {
            _typeIds = new Dictionary<int, int>(_typeIds?.Count ?? 0);
            for (int i = 0; i < _interactables.Count; i++)
            {
                Interactable interactable = _interactables[i];
                _typeIds.TryGetValue(interactable.InteractableTypeId, out int count);

                if (interactable.CanInteract && count < interactable.MaxActivations)
                {
                    interactable.Interact(this);
                    _typeIds[interactable.InteractableTypeId] = ++count;
                }
            }
        }
    }
}