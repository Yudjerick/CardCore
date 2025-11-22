using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

namespace CardCore
{
    public class CardDropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] SplineContainer spline;
        [SerializeField] HandCardContainer hand;

        private CardCoreManager _manager;

        private void Start()
        {
            _manager = FindAnyObjectByType<CardCoreManager>();
            if (_manager is null)
            {
                Debug.LogWarning("Couldn't find CardCoreManager");
            }
        }
        public void OnDrop(PointerEventData eventData)
        {
            _manager.DraggedCard.Remove();

            Vector3 localSplinePoint = spline.transform.InverseTransformPoint(_manager.DraggedCard.transform.position);
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
            

            hand.InsertCard(closestIndex, _manager.DraggedCard);


            
        }
    }
}

