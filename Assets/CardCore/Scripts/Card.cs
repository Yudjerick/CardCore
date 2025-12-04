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

        public UnityEvent<int> OnInitEvent;
        public UnityEvent OnSelectedEvent;
        public UnityEvent OnDeselectedEvent;
        public UnityEvent OnBeginDragEvent;
        public UnityEvent OnEndDragEvent;
        public UnityEvent OnRemovedEvent;

        private Vector3 _dragOffset;
        private bool recieveEvents = true;
        private GameObject _currentHowerTargetGameObject;
        private ICardHowerTarget _currentHowerTarget;
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
            GameObject targetGO = ExecuteEvents.GetEventHandler<ICardHowerTarget>(eventData.pointerCurrentRaycast.gameObject);
            if (targetGO != _currentHowerTargetGameObject)
            {
                _currentHowerTargetGameObject?.GetComponent<ICardHowerTarget>()?.OnHowerEnd(this);
                _currentHowerTargetGameObject = targetGO;
                _currentHowerTarget = targetGO?.GetComponent<ICardHowerTarget>();
                _currentHowerTarget?.OnHowerStart(this);
            }
            else
            {
                _currentHowerTarget?.OnHower(this);
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
            _currentHowerTargetGameObject?.GetComponent<ICardHowerTarget>()?.OnHowerEnd(this);
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
