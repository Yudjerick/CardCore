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
        public UnityEvent<int> OnInitEvent;
        public UnityEvent OnSelectedEvent;
        public UnityEvent OnDeselectedEvent;
        public UnityEvent OnBeginDragEvent;
        public UnityEvent OnEndDragEvent;

        private Vector3 _dragOffset;
        private bool recieveEvents = true;

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

            if (!draggable)
                return;
            transform.position = Camera.main.ScreenToWorldPoint(eventData.position) + _dragOffset;

        }

        public void OnBeginDrag(PointerEventData eventData)
        {

            if (!draggable)
                return;
            Dragged = true;
            transform.DOKill();
            _dragOffset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
            //Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            //Physics.Raycast(ray, out RaycastHit hit);
            //_dragOffset = transform.position - hit.point;

            OnBeginDragEvent?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            if (!draggable)
                return;
            Dragged = false;
            OnEndDragEvent?.Invoke();
        }
    }

}
