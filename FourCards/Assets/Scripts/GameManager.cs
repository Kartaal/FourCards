using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardValueEnum;
using static CardSuitEnum;


public class GameManager : MonoBehaviour
{
    [SerializeField] DeckController Deck;
    [SerializeField] DeckController DiscardPile;
    [SerializeField] DeckController PlayedPile;
    [SerializeField] PlayerController Player;
    [SerializeField] DeckController PlayerHand;
    // [SerializeField] AIController AI;
    [SerializeField] DeckController AIHand;
    [SerializeField] Card _CardPrefab;

    List<GameObject> AllCardObjects = new List<GameObject>();
    bool IsPlayersTurn = true;

    private void Awake() {
        InitGame();
    }

    private void InitGame() {
        GenerateCards();
        // DealFaceUpSets();
        DealHands();
    }

    private void GenerateCards() {
        ClearDeck();
        ClearDiscard();
        ClearPlayed();

        // Clear out any existing game objects --- QQQ should be handled by each deck
        foreach(GameObject cardGO in AllCardObjects)
        {
            Destroy(cardGO);
        }

        // Loop through card suits and card values to generate card objects of each combination
        foreach(CardSuitEnum suit in (CardSuitEnum[])Enum.GetValues(typeof(CardSuitEnum)))
        {
            foreach(CardValueEnum value in (CardValueEnum[])Enum.GetValues(typeof(CardValueEnum)))
            {
                Card card = Instantiate<Card>(_CardPrefab, _CardPrefab.transform.position, Quaternion.identity);
                card.SetCardFace(suit, value);

                Deck.Push(card);
                
                AllCardObjects.Add(card.gameObject);
                TransformCardToFitDeck(card, Deck);
            }
        }

        Deck.Shuffle();
    }

    private void DealHands() {
        // Debug.Log("Starting hand dealing...");
        PlayerDrawCards("Player", 4);
        PlayerDrawCards("AI", 4);
    }

    private void ClearDeck() {
        Deck.Clear();
    }

    private void ClearDiscard() {
        DiscardPile.Clear();
    }

    private void ClearPlayed() {
        PlayedPile.Clear();
    }

    public void _DebugDrawPlayerCard(int count) {
        PlayerDrawCards("Player", count);
    }

    public void _DebugDrawAICard(int count) {
        PlayerDrawCards("AI", count);
    }

    public void PlayerDrawCards(string id, int count) {
        // Draw the cards from the deck
        List<Card> drawn = Deck.PopMultiple(count);

        // Assign them correctly to player or AI
        if(id == "Player")
        {
            // Debug.Log($"Player getting {drawn.Count} cards");
            // foreach (var card in drawn)
            // {
            //     Debug.Log($"Card: {card.Value} of {card.Suit}");
            // }
            PlayerHand.PushMultiple(drawn);

            foreach (Card card in drawn)
            {
                TransformCardToFitHand(card, PlayerHand);
            }
        } else {
            // Debug.Log($"AI getting {drawn.Count} cards");
            AIHand.PushMultiple(drawn);

            foreach (Card card in drawn)
            {
                // Debug.Log($"Card: {card.Value} of {card.Suit}");
                TransformCardToFitHand(card, AIHand);
                card.FlipCard();
            }
        }
    }

    public void _DebugPlayFirstCardPlayer() {
        List<Card> card = PlayerHand.Pop();

        PlayCards(card);
    }

    public void PlayCards(List<Card> cards) {
        foreach (Card card in cards)
        {
            PlayedPile.Push(card);
            TransformCardToFitDeck(card, PlayedPile);
        }
    }


    void TransformCardToFitDeck(Card card, DeckController deck) {
        RectTransform cardTransform = card.gameObject.transform as RectTransform;

        // Ensure card object belongs to deck in hierarchy and visually
        cardTransform.SetParent(deck.gameObject.transform, false);

        // Need to set parent first for objects handled by layout groups - position is overridden
        cardTransform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        
        cardTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    void TransformCardToFitHand(Card card, DeckController hand) {
        RectTransform cardTransform = card.gameObject.transform as RectTransform;
        RectTransform _prefab = _CardPrefab.gameObject.transform as RectTransform;
        
        cardTransform.SetParent(hand.gameObject.transform, false);
        
        cardTransform.localScale = _prefab.localScale;
    }
}
