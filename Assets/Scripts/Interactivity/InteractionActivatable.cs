
using TMPro;
using UnityEngine;

namespace FunkyQuest
{
    internal class InteractionActivatable : Activatable
    {
        [Header("Interaction")]
        [SerializeField]    private GameObject  _sign;
        [SerializeField]    private TMP_Text    _keySign;
        [SerializeField]    private Rect        _rect;
        [SerializeField]    private LayerMask   _mask;
        [SerializeField]    private KeyCode     _key;

        private void Update()
        {
            if (HasActivated)
            {
                _sign.gameObject.SetActive(false);
            }
            else
            {
                Vector2 position = transform.position;
                RaycastHit2D raycast = Physics2D.BoxCast(position + _rect.position, _rect.size, 0, Vector2.zero, 0, _mask);

                bool canInteract = raycast.collider != null;
                _sign.gameObject.SetActive(canInteract);
                _keySign.SetText(_key.ToString());

                if (canInteract && Input.GetKeyDown(_key))
                {
                    Activate();
                }
            }
        }
    }
}