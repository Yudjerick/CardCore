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

        public UnityEvent<int> OnIndexUpdatedEvent;
        public UnityEvent OnSelectedEvent;
        public UnityEvent OnDeselectedEvent;
        public UnityEvent OnBeginDragEvent;
        public UnityEvent OnEndDragEvent;
        public UnityEvent OnRemovedEvent;
        public UnityEvent OnFocusStartEvent;
        public UnityEvent OnFocusEndEvent;

        private Vector3 _dragOffset;
        private bool recieveEvents = true;
        private GameObject _currentHoverTargetGameObject;
        private ICardHoverTarget _currentHoverTarget;
        private int _indexInContainer;

        private void Start()
        {
            if(CardCoreManager.Singleton is null)
            {
                Debug.LogWarning("Failed to access CardCoreManager, CardCore may behave incorrecntly");
                return;
            }
            CardCoreManager.Singleton.RegisterCard(this);
        }

        private void OnDestroy()
        {
            if (CardCoreManager.Singleton is null)
            {
                Debug.LogWarning("Failed to access CardCoreManager, CardCore may behave incorrecntly");
                return;
            }
            CardCoreManager.Singleton.UnregisterCard(this);
        }

        public void OnIndexUpdated(int index)
        {
            _indexInContainer = index;
            OnIndexUpdatedEvent?.Invoke(index);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectOnHower)
            {
                Selected = true;
                
                OnSelectedEvent?.Invoke();
                OnFocusStartEvent?.Invoke();

            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {

            if (selectOnHower)
            {
                
                Selected = false;
                OnDeselectedEvent?.Invoke();
                if (!Dragged)
                {
                    OnFocusEndEvent?.Invoke();
                }
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
            if (!Selected)
            {
                OnFocusStartEvent?.Invoke();
            }
            
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


            OnFocusEndEvent?.Invoke();
        }

        public void Remove()
        {
            OnRemovedEvent?.Invoke();
        }

        
    }

}
