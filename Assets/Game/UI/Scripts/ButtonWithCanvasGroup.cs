using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class ButtonWithCanvasGroup : Button
    {
        [SerializeField]
        CanvasGroup canvasGroup;

        public bool Interactable
        {
            get => !canvasGroup.enabled;
            set
            {
                canvasGroup.interactable = value;
                canvasGroup.enabled = !value;
            }
        }
    }
}
