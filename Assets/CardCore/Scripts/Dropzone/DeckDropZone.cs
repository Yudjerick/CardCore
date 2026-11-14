using UnityEngine;

namespace CardCore
{
    /// <summary>
    /// Drop target that puts the dropped card on top of a deck.
    /// </summary>
    public class DeckDropZone : MonoBehaviour, ICardDropTarget
    {
        [SerializeField] private DeckCardContainer deck;

        public void OnDrop(Card card)
        {
            if (deck is null || card is null)
            {
                return;
            }
            //card.container?.RemoveCard(card);
            //deck.PutCardOnTop(card);
        }

        public bool CanAccept(Card card)
        {
            return true;
        }
    }
}
