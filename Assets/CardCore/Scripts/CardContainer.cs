using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace CardCore
{
    public abstract class CardContainer : MonoBehaviour
    {
        [field: SerializeField]
        public List<Card> cards { get; private set; }

        private void Start()
        {
            OnUpdateCardsIndexes();
            for (int i = 0; i < cards.Count; i++)
            {
                OnCardAdded(cards[i]);
            }
            UpdateChidrenTransforms();
        }

        protected abstract void UpdateChidrenTransforms();


        protected void OnUpdateCardsIndexes()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].OnIndexUpdated(i);
            }
        }

        protected void DisableRecieveCardEvents()
        {
            foreach (var card in cards)
            {
                card.RecieveEvents = false;
            }
        }

        protected void EnableRecieveCardEvents()
        {
            foreach (var card in cards)
            {
                card.RecieveEvents = true;
            }
        }

        protected virtual void OnCardAdded(Card card)
        {
            card.OnSelectedEvent.AddListener(UpdateChidrenTransforms);
            card.OnDeselectedEvent.AddListener(UpdateChidrenTransforms);
            card.OnBeginDragEvent.AddListener(DisableRecieveCardEvents);
            card.OnEndDragEvent.AddListener(EnableRecieveCardEvents);
            card.OnRemovedEvent.AddListener(() => OnCardRemoved(card));
        }

        public virtual void InsertCard(int index, Card card)
        {
            cards.Insert(index, card);
            OnCardAdded(card);
            OnUpdateCardsIndexes();
            UpdateChidrenTransforms();
        }

        protected virtual void OnCardRemoved(Card card)
        {
            cards.Remove(card);
            card.OnSelectedEvent.RemoveListener(UpdateChidrenTransforms);
            card.OnDeselectedEvent.RemoveListener(UpdateChidrenTransforms);
            card.OnBeginDragEvent.RemoveListener(DisableRecieveCardEvents);
            card.OnEndDragEvent.RemoveListener(EnableRecieveCardEvents);
            card.OnRemovedEvent.RemoveAllListeners();
            UpdateChidrenTransforms();
        }
    }

}
