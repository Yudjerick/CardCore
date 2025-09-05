using CardCore;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardCore
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private bool selectOnHower;
        [SerializeField] private bool draggable;
        public bool Selected { get; private set; }
        public bool Dragged { get; private set; }

        public UnityEvent<int> OnInitEvent;
        public UnityEvent OnSelectedEvent;
        public UnityEvent OnDeselectedEvent;

        private Vector3 _dragOffset;

        public void Init(int index)
        {
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
            if(!draggable)
                return;
            Dragged = true;
            transform.position = Camera.main.ScreenToWorldPoint(eventData.position) + _dragOffset;

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!draggable)
                return;
            transform.DOKill();
            print(Camera.main.ScreenToWorldPoint(eventData.position));
            _dragOffset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!draggable)
                return;
            Dragged = false;
        }
    }

}
