
using TMPro;
using UnityEngine;

namespace FunkyQuest
{
    internal class InputActivatable : Activatable
    {
        [Header("Input")]
        [SerializeField]    private string          _acceptedString;
        [SerializeField]    private TMP_InputField  _inputField;

        private void Start()
        {
            if (_inputField != null)
            {
                _inputField.onValueChanged.AddListener(
                    value =>
                    {
                        if (value.ToLower() == _acceptedString.ToLower() && !HasActivated)
                        {
                            Activate();
                        }
                    }
                );
            }
        }
    }
}