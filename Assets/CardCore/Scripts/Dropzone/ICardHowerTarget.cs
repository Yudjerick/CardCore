using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace CardCore
{
    public interface ICardHowerTarget : IEventSystemHandler
    {
        void OnHowerStart(Card card);

        void OnHower(Card card);

        void OnHowerEnd(Card card);
    }
}
