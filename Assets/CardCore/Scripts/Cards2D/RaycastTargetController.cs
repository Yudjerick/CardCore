using UnityEngine;
using UnityEngine.UI;

namespace CardCore
{
    /// <summary>
    /// Enables and disables Raycast Target on Cards that use MaskableGraphics for correct IDropHandler work
    /// </summary>
    [RequireComponent(typeof(MaskableGraphic))]
    [RequireComponent(typeof(Card))]
    public class RaycastTargetController : MonoBehaviour
    {

        private Card _layoutElementCard;
        private MaskableGraphic _graphics;

        private void Awake()
        {
            _layoutElementCard = GetComponent<Card>();
            _graphics = GetComponent<MaskableGraphic>();

            _layoutElementCard.OnBeginDragEvent.AddListener(OnBeginDrag);
            _layoutElementCard.OnEndDragEvent.AddListener(OnEndDrag);
        }

        public void OnBeginDrag()
        {
            _graphics.raycastTarget = false;
        }

        public void OnEndDrag()
        {
            _graphics.raycastTarget = true;
        }
    }
}