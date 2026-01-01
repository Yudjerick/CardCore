using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace CardCore
{
    public class HandCardContainer : CardContainer
    {

        //[field: SerializeField]
        //public List<Card> cards { get; private set; }
        public List<float> cardsSplineT { get; private set; } = new List<float>();
        [SerializeField] private float cardSpacing;
        [SerializeField] private float moveDuration;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private Vector3 cardScale;
        [SerializeField] private float offsetZ;

        [Header("Selected card settings")]
        [SerializeField] private float selectedSpacingMultiplier;
        [SerializeField] private float selectedScaleMultiplier;
        [SerializeField][Range(0f,1f)] private float straigntenSelectedRotation;
        [SerializeField] private Vector3 selectedLocalOffset;
        [SerializeField] private bool useAnotherSplineForSelected;
        [SerializeField] private SplineContainer selectedSplineContainer;
        [SerializeField] private float selectedMoveDuration;

        /*private void Start()
        {
            OnUpdateCardsIndexes();
            for(int i = 0; i < cards.Count; i++) 
            {
                OnCardAdded(cards[i]);
            }
            UpdateChidrenTransforms();
        }*/

        protected override void UpdateChidrenTransforms()
        {
            RecalculateCardsT();
            for(int i = 0; i < cards.Count; i++)
            {
                SplineContainer currentSpline = splineContainer;
                float currentMoveDuration;
                if (cards[i].Selected)
                {
                    currentMoveDuration = selectedMoveDuration;
                    if (useAnotherSplineForSelected)
                    {
                        currentSpline = selectedSplineContainer;
                    }
                }
                else
                {
                    currentMoveDuration = moveDuration;
                }

                if (cards[i].Dragged)
                {
                    continue;
                }

                Vector3 cardPosition;
                float3 splinePosition;
                float3 splineTangent;
                currentSpline.Evaluate(cardsSplineT[i], out splinePosition, out splineTangent, out float3 _);
                
                cardPosition = splinePosition;
                cardPosition.z += offsetZ * i;
                if (cards[i].Selected)
                {
                    cardPosition += selectedLocalOffset;
                }
                cards[i].transform.DOKill();
                cards[i].transform.DOMove(cardPosition, currentMoveDuration);

                Vector3 axis = transform.forward;
                float angle = Vector3.SignedAngle(transform.right, splineTangent, axis);
                angle = cards[i].Selected ? angle * (1 - straigntenSelectedRotation) : angle;
                cards[i].transform.DORotate(axis * angle + transform.rotation.eulerAngles, currentMoveDuration);
                cards[i].transform.DOScale(cardScale * (cards[i].Selected ? selectedScaleMultiplier : 1f), currentMoveDuration);
            }
        }

        private void RecalculateCardsT()
        {
            float fixedCardSpacing = cardSpacing;
            float fixedSelectedCardSpacing = cardSpacing * selectedSpacingMultiplier;
            int selectedCardsCount = cards.Count(card => card.Selected);
            float cardZoneLength = cards.Sum(card => card.Selected ? fixedSelectedCardSpacing : cardSpacing); //Make desired spacing property in card?;
            if (cardZoneLength > 1f)
            {
                cardZoneLength = 1f;
                fixedCardSpacing = 1f / ((cards.Count - selectedCardsCount) + selectedCardsCount * selectedSpacingMultiplier);
                fixedSelectedCardSpacing = fixedCardSpacing * selectedSpacingMultiplier;
            }
            float offsetZoneLength = (1f - cardZoneLength) / 2f;

            float alpha = offsetZoneLength;
            for (int i = 0; i < cards.Count; i++)
            {
                float currentCardMargin;
                if (cards[i].Selected)
                {
                    currentCardMargin = fixedSelectedCardSpacing / 2f;
                }
                else
                {
                    currentCardMargin = fixedCardSpacing / 2f;
                }

                alpha += currentCardMargin;
                cardsSplineT[i] = alpha;
                alpha += currentCardMargin;
            }
        }

        protected override void OnCardAdded(Card card)
        {
            base.OnCardAdded(card);
            cardsSplineT.Add(0f);
        }

        protected override void OnCardRemoved(Card card)
        {
            cards.Remove(card);
            card.OnSelectedEvent.RemoveListener(UpdateChidrenTransforms);
            card.OnDeselectedEvent.RemoveListener(UpdateChidrenTransforms);
            card.OnBeginDragEvent.RemoveListener(DisableRecieveCardEvents);
            card.OnEndDragEvent.RemoveListener(EnableRecieveCardEvents);
            card.OnRemovedEvent.RemoveAllListeners();
            
            cardsSplineT.RemoveAt(0);
            UpdateChidrenTransforms();
        }
    }
}

