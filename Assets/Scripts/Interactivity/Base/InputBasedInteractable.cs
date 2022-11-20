
using UnityEngine;

namespace FunkyQuest
{
    internal class InputBasedInteractable : Interactable
    {
        public enum InteractionState
        {
            None        = 0,
            Activating  = 1,
            Activated   = 2,
            Canceling   = 3
        }

        [Header("Input Based Interactable - Read Only")]
        [SerializeField][ReadOnly]  private InteractionState        _state;
        [SerializeField][ReadOnly]  private int                     _effectorActivationsCount;

        [Header("Input Based Interactable - Properties")]
        [SerializeField]            private int                     _maxEffectorActivationsCount;
        [SerializeField]            private InteractableEffector[]  _activating;
        [SerializeField]            private InteractableEffector[]  _canceling;
        [SerializeField]            private KeyCode                 _activationKey;
        [SerializeField]            private KeyCode                 _cancelationKey;

        public override void Interact(CharacterInteractivity triggerSource)
        {
            InteractionState current = _state;
            UpdateState(
                InteractionState.None,
                InteractionState.Activating,
                InteractionState.Activated,
                in _activationKey,
                _activating
            );

            if (current == _state)
            {
                UpdateState(
                    InteractionState.Activated,
                    InteractionState.Canceling,
                    InteractionState.None,
                    in _cancelationKey,
                    _canceling
                );
            }
        }

        private void UpdateState(InteractionState interact, InteractionState next, InteractionState finished, in KeyCode keyCode, InteractableEffector[] effectors)
        {
            if (_state == interact)
            {
                bool areEffectorsActivated = false;
                for (int i = 0; i < effectors.Length && !areEffectorsActivated; i++)
                {
                    areEffectorsActivated = effectors[0].IsActivated;
                }

                bool canInteract =
                    !areEffectorsActivated &&
                    (_effectorActivationsCount < _maxEffectorActivationsCount || _maxEffectorActivationsCount == -1) &&
                    keyCode != KeyCode.None &&
                    Input.GetKeyDown(keyCode);

                if (canInteract)
                {
                    _state = next;
                    _effectorActivationsCount++;

                    for (int i = 0; i < effectors.Length; i++)
                    {
                        InteractableEffector effector = effectors[i];
                        effector.Finished += (InteractableEffector e) => OnEffectorFinished(finished, e);
                        effector.PerformEffect();
                    }
                }
            }
        }

        private void OnEffectorFinished(InteractionState state, InteractableEffector effector)
        {
            _state = state;
            effector.Finished -= (InteractableEffector effector) => OnEffectorFinished(state, effector);
        }
    }
}