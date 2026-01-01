using DG.Tweening;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

namespace CardCore
{
    public class CardDropZone : MonoBehaviour, ICardDropTarget, ICardHoverTarget
    {
        [SerializeField] SplineContainer spline;
        [SerializeField] HandCardContainer hand;
        [SerializeField] Card howerHighlightEffectPrefab;

        private Card _howerEffectInstance;
        private int _indexToInsert;

        public void OnDrop(Card card)
        {

            _indexToInsert = CalculateClosestIndex(card);
            if(hand.cards.Contains(card) && card.IndexInContainer < _indexToInsert)
            {
                _indexToInsert--;
            }
            card.Remove();

            hand.InsertCard(_indexToInsert, card);
            _indexToInsert = -1;

            
        }

        public void OnHoverStart(Card card)
        {
        }

        public bool CanAccept(Card card)
        {
            return true;
        }

        public void OnHoverEnd(Card card)
        {
            _howerEffectInstance?.Remove();
            Destroy(_howerEffectInstance?.gameObject);
            _howerEffectInstance = null;
        }

        public void OnHover(Card card)
        {
            int closestIndex = CalculateClosestIndex(card);

            if(_indexToInsert == closestIndex)
            {
                return;
            }

            _howerEffectInstance?.Remove();
            Destroy(_howerEffectInstance?.gameObject);
            _howerEffectInstance = Instantiate(howerHighlightEffectPrefab);
            hand.InsertCard(closestIndex, _howerEffectInstance);
            _indexToInsert = closestIndex;
            _howerEffectInstance.transform.DOComplete();
        }

        private int CalculateClosestIndex(Card card)
        {
            int closestIndex = 0;
            if (hand.cards.Count > 0)
            {
                Vector3 localSplinePoint = spline.transform.InverseTransformPoint(card.transform.position);
                SplineUtility.GetNearestPoint(spline.Spline, localSplinePoint, out float3 _, out float t);

                while (t > hand.cardsSplineT[closestIndex])
                {
                    closestIndex++;
                    if (closestIndex == hand.cardsSplineT.Count)
                    {
                        break;
                    }
                }

                if (_howerEffectInstance is not null && _howerEffectInstance.IndexInContainer < closestIndex)
                {
                    closestIndex--;
                }
            }
            return closestIndex;
        }
    }
}

