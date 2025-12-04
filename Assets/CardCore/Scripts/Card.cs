using CardCore;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardCore
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {

        [SerializeField] private bool selectOnHower;
        [SerializeField] private bool draggable;
        public bool Selected { get; private set; }
        public bool Dragged { get; private set; }
        public bool RecieveEvents { get => recieveEvents; set
            {
                GetComponent<MaskableGraphic>().raycastTarget = value;
                //recieveEvents = value;
            } 
        }

        public int IndexInContainer { get => _indexInContainer; set => _indexInContainer = value; }
        public HandCardContainer container { get; private set; }

        public UnityEvent<int> OnInitEvent;
        public UnityEvent OnSelectedEvent;
        public UnityEvent OnDeselectedEvent;
        public UnityEvent OnBeginDragEvent;
        public UnityEvent OnEndDragEvent;
        public UnityEvent OnRemovedEvent;

        private Vector3 _dragOffset;
        private bool recieveEvents = true;
        private GameObject _currentHoverTargetGameObject;
        private ICardHoverTarget _currentHoverTarget;
        private int _indexInContainer;


        public void Init(int index)
        {
            _indexInContainer = index;
            OnInitEvent?.Invoke(index);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectOnHower)
            {
                Selected = true;
                OnSelectedEvent?.Invoke();
                
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {

            if (selectOnHower)
            {
                Selected = false;
                OnDeselectedEvent?.Invoke();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {

            if (!draggable)
                return;
            transform.position = Camera.main.ScreenToWorldPoint(eventData.position) + _dragOffset;
            GameObject targetGO = ExecuteEvents.GetEventHandler<ICardHoverTarget>(eventData.pointerCurrentRaycast.gameObject);
            if (targetGO != _currentHoverTargetGameObject)
            {
                _currentHoverTargetGameObject?.GetComponent<ICardHoverTarget>()?.OnHoverEnd(this);
                _currentHoverTargetGameObject = targetGO;
                _currentHoverTarget = targetGO?.GetComponent<ICardHoverTarget>();
                _currentHoverTarget?.OnHoverStart(this);
            }
            else
            {
                _currentHoverTarget?.OnHover(this);
            }

        }

        public void OnBeginDrag(PointerEventData eventData)
        {

            if (!draggable)
                return;
            Dragged = true;
            transform.DOKill();
            _dragOffset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);

            OnBeginDragEvent?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            if (!draggable)
                return;
            Dragged = false;
            OnEndDragEvent?.Invoke();
            _currentHoverTargetGameObject?.GetComponent<ICardHoverTarget>()?.OnHoverEnd(this);
            GameObject target = ExecuteEvents.GetEventHandler<ICardDropTarget>(eventData.pointerCurrentRaycast.gameObject);
            if (target != null)
            {
                target.GetComponent<ICardDropTarget>().OnDrop(this);
            }
        }

        public void Remove()
        {
            OnRemovedEvent?.Invoke();
        }
    }

}
