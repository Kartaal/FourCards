using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardValueEnum;
using static CardSuitEnum;
using static GameStateEnum;


public class GameManager : MonoBehaviour
{
    [SerializeField] DeckController Deck;
    [SerializeField] DeckController DiscardDeck;
    [SerializeField] PlayedDeckController PlayedDeck;
    [SerializeField] PlayerController Player;
    // // [SerializeField] HandDeckController PlayerHand;
    // [SerializeField] AIController AI;
    [SerializeField] HandDeckController AIHand;
    [SerializeField] Card _CardPrefab;

    List<GameObject> AllCardObjects = new List<GameObject>();
    public static GameStateEnum GameState = Init;

    private void Start() {
        InitGame();
    }

    private void InitGame() {
        GenerateCards();
        DealFaceUpSets();
        DealHands();
        GameState = SwappingCards; // Make function for this
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
                // TransformCardToFitDeck(card, Deck);
            }
        }

        Deck.Shuffle();
    }

    private void DealHands() {
        // Debug.Log("Starting hand dealing...");
        PlayerDrawCards("Player", 4);
        PlayerDrawCards("AI", 4);
    }

    private void DealFaceUpSets() {
        List<Card> PlayerEight = Deck.PopMultiple(8);
        List<Card> AIEight = Deck.PopMultiple(8);

        Player.InitFinalSets(PlayerEight);
        // AI.InitFinalSets(AIEight);

        // For debugging things...
        foreach (Card AICard in AIEight)
        {
            Destroy(AICard.gameObject);
        }
    }

    public void DiscardCards(List<Card> cardsToDiscard, GameObject source) {
        if(source.name == "PlayedDeck")
        {
            DiscardDeck.PushMultiple(cardsToDiscard);
            ClearDiscard();
        }
    }

    private void ClearDeck() {
        Deck.Clear();
    }

    private void ClearDiscard() {
        DiscardDeck.Clear();
    }

    private void ClearPlayed() {
        PlayedDeck.Clear();
    }

    public void _DebugDrawPlayerCard(int count) {
        PlayerDrawCards("Player", count);
    }

    public void _DebugDrawAICard(int count) {
        PlayerDrawCards("AI", count);
    }

    public void PlayerDrawCards(string id, int count) {
        // Draw the cards from the deck
        // Debug.Log($"Drawing cards for player, {Deck.ReturnRemainingCardCount()} remaining...");
        count = Math.Min(Deck.ReturnRemainingCardCount(), count);
        List<Card> drawn = Deck.PopMultiple(count);

        // Assign them correctly to player or AI
        if(id == "Player")
        {
            // Debug.Log($"Player getting {drawn.Count} cards");
            // foreach (var card in drawn)
            // {
            //     Debug.Log($"Card: {card.Value} of {card.Suit}");
                // Debug.Log($"Player card face up: {card.IsFaceUp}");
            // }
            Player.AddCardsToHand(drawn);
        } else {
            // Debug.Log($"AI getting {drawn.Count} cards");
            AIHand.PushMultiple(drawn);     // QQQ Fix this so it uses the AI controller
            // AI.AddCardsToHand(drawn);

            foreach (Card card in drawn)
            {
                // Debug.Log($"AI card face up: {card.IsFaceUp}");
                // Debug.Log($"Card: {card.Value} of {card.Suit}");
                card.FlipCard();
            }
        }
    }

    public void _DebugPlayFirstCardPlayer() {
        List<Card> card = Player._DebugPopFirstCard();

        PlayCards(card);
    }

    public void PlayCards(List<Card> cards) {
        foreach (Card card in cards)
        {
            PlayedDeck.Push(card);
            // TransformCardToFitDeck(card, PlayedDeck);
        }
    }
}
