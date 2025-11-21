using UnityEngine;
using UnityEngine.EventSystems;

namespace CardCore
{
    public class CardDropZone : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            print("droped");
        }
    }
}

