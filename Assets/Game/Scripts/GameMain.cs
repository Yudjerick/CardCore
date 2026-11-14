using System.Collections.Generic;
using CardCore;
using Cysharp.Threading.Tasks;
using UnityEngine;
public class GameMain : MonoBehaviour
{
    [SerializeField] private HandCardContainer hand;
    [SerializeField] private List<Card> deck;
    [SerializeField] private int cardsPerTurn;
        
    void Start() 
    {
        Main();
    }

    async UniTask Main()
    {
        PreparePlayerTurn();
    }

    async UniTask PreparePlayerTurn()
    {
        for (int i = 0; i < cardsPerTurn; i++)
        {
            var instance = Instantiate(deck[i]);
            hand.InsertCard(0, instance);
            await UniTask.WaitForSeconds(0.3f);
        }
        
    }
}
