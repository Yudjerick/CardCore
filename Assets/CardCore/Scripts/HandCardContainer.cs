using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace CardCore
{
    public class HandCardContainer : CardContainer
    {

        [SerializeField]
        private List<Card> cards;
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

        private void Start()
        {
            OnUpdateCardsIndexes();
            for(int i = 0; i < cards.Count; i++) 
            {
                SetCardCallbacks(cards[i]);
            }
            UpdateChidrenTransforms();
        }

        private void UpdateChidrenTransforms()
        {
            float fixedCardSpacing = cardSpacing;
            float fixedSelectedCardSpacing = cardSpacing * selectedSpacingMultiplier;
            int selectedCardsCount = cards.Count(card => card.Selected);
            float cardZoneLength = cards.Sum(card => card.Dragged ? 0f : card.Selected ? fixedSelectedCardSpacing : cardSpacing); //Make desired spacing property in card?;
            if(cardZoneLength > 1f)
            {
                cardZoneLength = 1f;
                fixedCardSpacing = 1f / ((cards.Count - selectedCardsCount) + selectedCardsCount * selectedSpacingMultiplier);
                fixedSelectedCardSpacing = fixedCardSpacing * selectedSpacingMultiplier;
            }
            float offsetZoneLength = (1f - cardZoneLength) / 2f;

            float alpha = offsetZoneLength;
            for(int i = 0; i < cards.Count; i++)
            {
                float currentCardMargin;
                SplineContainer currentSpline = splineContainer;
                float currentMoveDuration;
                if (cards[i].Selected)
                {
                    currentMoveDuration = selectedMoveDuration;
                    currentCardMargin = fixedSelectedCardSpacing / 2f;
                    if (useAnotherSplineForSelected)
                    {
                        currentSpline = selectedSplineContainer;
                    }
                }
                else
                {
                    currentMoveDuration = moveDuration;
                    currentCardMargin = fixedCardSpacing / 2f;
                }
                

                if (cards[i].Dragged)
                {
                    continue;
                }

                alpha += currentCardMargin;

                Vector3 cardPosition;
                float3 splinePosition;
                float3 splineTangent;
                currentSpline.Evaluate(alpha, out splinePosition, out splineTangent, out float3 _);
                
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

                alpha += currentCardMargin;
            }
        }

        private void OnUpdateCardsIndexes()
        {
            for(int i = 0; i < cards.Count; i++)
            {
                cards[i].Init(i);
            }
        }

        private void DisableRecieveCardEvents()
        {
            foreach(var card in cards)
            {
                card.RecieveEvents = false;
            }
        }

        private void EnableRecieveCardEvents()
        {
            foreach (var card in cards)
            {
                card.RecieveEvents = true;
            }
        }

        private void SetCardCallbacks(Card card)
        {
            card.OnSelectedEvent.AddListener(UpdateChidrenTransforms);
            card.OnDeselectedEvent.AddListener(UpdateChidrenTransforms);
            card.OnBeginDragEvent.AddListener(DisableRecieveCardEvents);
            card.OnEndDragEvent.AddListener(EnableRecieveCardEvents);
        }

        public void InsertCard(int index, Card card)
        {
            cards.Insert(index, card);
            SetCardCallbacks(card);
            OnUpdateCardsIndexes();
            UpdateChidrenTransforms();
        }
    }
}

