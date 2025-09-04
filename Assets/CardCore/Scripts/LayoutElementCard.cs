using CardCore;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LayoutElementCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool selectOnHower;
    public bool Selected { get; private set; }

    public UnityEvent<int> OnInitEvent;
    public UnityEvent OnSelectedEvent;
    public UnityEvent OnDeselectedEvent;

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
}
