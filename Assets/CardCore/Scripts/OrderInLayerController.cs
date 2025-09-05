using UnityEngine;

namespace CardCore
{
    [RequireComponent(typeof(Card))]
    [RequireComponent(typeof(Canvas))]
    public class OrderInLayerController : MonoBehaviour
    {
        [SerializeField] private int firstCardOrderInLayer;
        [SerializeField][Range(-1, 1)] private int orderInLayerStep;
        [SerializeField] private int selectedOrderInLayer;

        private int _initialOrderInLayer;
        private Canvas _canvas;
        private Card _layoutElementCard;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _layoutElementCard = GetComponent<Card>();
            _layoutElementCard.OnInitEvent.AddListener(OnInit);
            _layoutElementCard.OnSelectedEvent.AddListener(OnSelected);
            _layoutElementCard.OnDeselectedEvent.AddListener(OnDeselected);
        }
        public void OnInit(int index)
        {
            _initialOrderInLayer = firstCardOrderInLayer + orderInLayerStep * index;
            _canvas.sortingOrder = _initialOrderInLayer;
        }

        public void OnSelected()
        {
            _canvas.sortingOrder = selectedOrderInLayer;
        }

        public void OnDeselected()
        {
            _canvas.sortingOrder = _initialOrderInLayer;
        }
    }
}

