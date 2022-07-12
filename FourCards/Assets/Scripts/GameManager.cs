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
    bool isPlayersTurn = true;

    private void Awake() {
        GenerateCards();
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
                TransformCardToFitDeck(card);
            }
        }

        Deck.Shuffle();
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

    public void DrawPlayerCardDebug(int count) {
        PlayerDrawCards("Player", count);
    }

    public void DrawAICardDebug(int count) {
        PlayerDrawCards("AI", count);
    }

    public void PlayerDrawCards(string id, int count) {
        // Draw the cards from the deck
        List<Card> drawn = Deck.PopMultiple(count);

        // Assign them correctly to player or AI
        if(id == "Player")
        {
            PlayerHand.PushMultiple(drawn);

            foreach (Card card in drawn)
            {
                TransformCardToFitHand(card, PlayerHand);
            }
        } else {
            AIHand.PushMultiple(drawn);

            foreach (Card card in drawn)
            {
                TransformCardToFitHand(card, AIHand);
                card.FlipCard();
            }
        }
    }


    void TransformCardToFitDeck(Card card) {
        RectTransform cardTransform = card.gameObject.transform as RectTransform;

        // Ensure card object belongs to deck in hierarchy and visually
        cardTransform.SetParent(Deck.gameObject.transform, false);
        
        cardTransform.anchorMin = new Vector2(0.5f, 0.5f);
        cardTransform.anchorMax = new Vector2(0.5f, 0.5f);
        
        cardTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    void TransformCardToFitHand(Card card, DeckController hand) {
        RectTransform cardTransform = card.gameObject.transform as RectTransform;
        RectTransform _prefab = _CardPrefab.gameObject.transform as RectTransform;
        
        cardTransform.SetParent(hand.gameObject.transform, false);
        
        cardTransform.anchorMin = _prefab.anchorMin;
        cardTransform.anchorMax = _prefab.anchorMax;
        
        cardTransform.localScale = _prefab.localScale;
    }
}
