using UnityEngine;
using UnityEngine.EventSystems;

namespace CardCore
{
    public interface IDropTarget: IEventSystemHandler
    {
        void OnDrop(Card card);
    }
}

