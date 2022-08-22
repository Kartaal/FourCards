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

    public virtual void PushMultiple(List<Card> newCards) {
        foreach (Card card in newCards)
        {
            Cards.Add(card);
            TransformCardToFitDeck(card);
        }

        if(LayoutController != null) {
            LayoutController.UpdateSpacing(Cards.Count);
        }
    }

    public List<Card> Pop() {
        return PopMultiple(1);
    }

    public List<Card> PopMultiple(int count) {
        // Debug.Log($"Popping {count} cards from {gameObject.name} with {Cards.Count} remaining");
        // Ensure not popping more than count
        count = Math.Min(Cards.Count, count);

        List<Card> poppedCards = new List<Card>();
        while (count > 0 && Cards.Count > 0)
        {
            // Debug.Log($"Fiddling while index is {Cards.Count-1}");
            Card card = Cards[Cards.Count-1];
            Cards.RemoveAt(Cards.Count-1);

            if(Cards.Count > 0)
                Cards[Cards.Count-1].FlipCard();

            poppedCards.Add(card);
            count--;
        }

        // Debug.Log($"After popping... {Cards.Count} cards remaining");

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

        // Remember to update the game object ordering to match the shuffled list
        for (int index = 0; index < Cards.Count; index++)
        {
            Card card = Cards[index];
            RectTransform cardTransform = card.transform as RectTransform;

            cardTransform.SetSiblingIndex(index);
            if(card.IsFaceUp) {
                card.FlipCard();
            }
        }
        Cards[Cards.Count-1].FlipCard();

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

    public int ReturnRemainingCardCount() {
        return Cards.Count;
    }

    private void TransformCardsToFitDeck(List<Card> cards) {
        foreach (Card card in cards)
        {
            TransformCardToFitDeck(card);
        }
    }

    private void TransformCardToFitDeck(Card card) {
        RectTransform cardTransform = card.gameObject.transform as RectTransform;

        // Ensure card object belongs to deck in hierarchy and visually
        cardTransform.SetParent(gameObject.transform, false);

        // Need to set parent first for objects handled by layout groups - position is overridden
        cardTransform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        
        cardTransform.localScale = new Vector3(1f, 1f, 1f);
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
