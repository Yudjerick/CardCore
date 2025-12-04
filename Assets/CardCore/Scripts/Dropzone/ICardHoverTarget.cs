using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace CardCore
{
    public interface ICardHoverTarget : IEventSystemHandler
    {
        void OnHoverStart(Card card);

        void OnHover(Card card);

        void OnHoverEnd(Card card);
    }
}
