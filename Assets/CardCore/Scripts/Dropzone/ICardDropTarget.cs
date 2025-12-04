using UnityEngine;
using UnityEngine.EventSystems;

namespace CardCore
{
    public interface ICardDropTarget: IEventSystemHandler
    {
        void OnDrop(Card card);



        bool CanAccept(Card card);
    }
}

