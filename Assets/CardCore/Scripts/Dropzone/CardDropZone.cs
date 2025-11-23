using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

namespace CardCore
{
    public class CardDropZone : MonoBehaviour, IDropTarget
    {
        [SerializeField] SplineContainer spline;
        [SerializeField] HandCardContainer hand;

        //private CardCoreManager _manager;

        private void Start()
        {
            /*_manager = FindAnyObjectByType<CardCoreManager>();
            if (_manager is null)
            {
                Debug.LogWarning("Couldn't find CardCoreManager");
            }*/
        }
        public void OnDrop(Card card)
        {
            card.Remove();

            Vector3 localSplinePoint = spline.transform.InverseTransformPoint(card.transform.position);
            SplineUtility.GetNearestPoint(spline.Spline, localSplinePoint, out float3 _, out float t);
            int closestIndex = 0;
            while (t > hand.cardsSplineT[closestIndex])
            {
                closestIndex++;
                if(closestIndex == hand.cardsSplineT.Count)
                {
                    break;
                }
            }
            

            hand.InsertCard(closestIndex, card);


            
        }
    }
}

