using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardValueEnum;
using static CardSuitEnum;


public class DeckController : MonoBehaviour
{
    [SerializeField] bool DisplayDebug;
    [SerializeField] protected GameManager Manager;
    GroupLayoutController LayoutController;

    protected List<Card> Cards = new List<Card>();

    private void Awake() {
        GroupLayoutController layoutCtrl = GetComponent<GroupLayoutController>();

        if(layoutCtrl)
            LayoutController = layoutCtrl;
    }

    public void Push(Card newCard) {
        PushMultiple(new List<Card>() {newCard});
    }

    public void PushMultiple(List<Card> newCards) {
        foreach (Card card in newCards)
        {
            Cards.Add(card);
        }

        if(LayoutController != null) {
            LayoutController.UpdateSpacing(Cards.Count);
        }
    }

    public List<Card> Pop() {
        return PopMultiple(1);
    }

    public List<Card> PopMultiple(int count) {
        // Ensure not popping more than count
        if(count > Cards.Count)
        {
            count = Cards.Count;
        }

        List<Card> poppedCards = new List<Card>();
        while (count > 0)
        {
            Card card = Cards[Cards.Count-1];
            Cards.RemoveAt(Cards.Count - 1);

            poppedCards.Add(card);
            count--;
        }

        return poppedCards;
    }

    public List<Card> PopSelected(List<Card> selected) {
        // Get all cards matching the suit and value (card) in selected list
        // Horrible handling with list but oh well
        IEnumerable<Card> poppedCards = from c in Cards
                                 join s in selected on new 
                                 { 
                                    c.Value,
                                    c.Suit 
                                 } equals new
                                 { s.Value,
                                   s.Suit
                                 }
                                 select c;
        
        // Remove all the popped cards from the internal list
        Cards.RemoveAll(card => poppedCards.Contains(card));

        return poppedCards as List<Card>;
    }

    public void Shuffle() {
        System.Random rand = new System.Random();

        // if(DisplayDebug)
        // {
        //     List<(CardSuitEnum, CardValueEnum)> displayList = new List<(CardSuitEnum, CardValueEnum)>();
        //     Debug.Log("Before shuffle:");
        //     foreach(Card card in Cards)
        //     {
        //         (CardSuitEnum, CardValueEnum) tuple = (card.Suit, card.Value);
        //         displayList.Add(tuple);
        //     }
        //     Debug.Log(displayList);
        // }

        for (int i = Cards.Count - 1; i > 0; i--)
        {
            int j = rand.Next(0, i+1);
            
            Card temp = Cards[i];
            Cards[i] = Cards[j];
            Cards[j] = temp;
        }

        // if(DisplayDebug)
        // {
        //     List<(CardSuitEnum, CardValueEnum)> displayList = new List<(CardSuitEnum, CardValueEnum)>();
        //     Debug.Log("After shuffle:");
        //     foreach(Card card in Cards)
        //     {
        //         (CardSuitEnum, CardValueEnum) tuple = (card.Suit, card.Value);
        //         displayList.Add(tuple);
        //     }
        //     Debug.Log(displayList);
        // }
    }

    public void Clear() {
        Cards.Clear();
    }

    public Card Peek() {
        return Cards[Cards.Count-1];
    }

    public (Card, int) PeekMatching() {
        int index = Cards.Count - 1;
        Card topCard = Cards[index];
        CardValueEnum value = topCard.Value;

        Card toLookAt = Cards[index];
        int count = 0;

        while(toLookAt.Value == topCard.Value)
        {
            count++;
            toLookAt = Cards[--index];
        }

        if(DisplayDebug)
        {
            Debug.Log($"Peeking matching in deck... found {count} of {value}");
        }

        return (topCard, count);
    }
}
