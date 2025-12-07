
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class for global management 
/// </summary>
namespace CardCore
{
    public class CardCoreManager : MonoBehaviour
    {
        public static CardCoreManager Singleton;

        private List<Card> cardsOnScene = new List<Card>();

        private void Awake()
        {
            Singleton = this;
        }

        public void RegisterCard(Card card)
        {
            cardsOnScene.Add(card);
            card.OnBeginDragEvent.AddListener(OnAnyCardBeginDrag);
            card.OnEndDragEvent.AddListener(OnAnyCardEndDrag);
        }

        public void UnregisterCard(Card card)
        {
            cardsOnScene.Remove(card);
            card.OnBeginDragEvent.RemoveListener(OnAnyCardBeginDrag);
            card.OnEndDragEvent.RemoveListener(OnAnyCardEndDrag);
        }

        public void OnAnyCardBeginDrag()
        {
            foreach (Card card in cardsOnScene)
            {
                card.RecieveEvents = false;
            }
        }
        public void OnAnyCardEndDrag()
        {
            foreach (Card card in cardsOnScene)
            {
                card.RecieveEvents = true;
            }
        }

    }
}

