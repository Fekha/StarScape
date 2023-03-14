using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager i;
    public DeckManagerCard cardPrefab;
    public GameObject deckArea;
    public GameObject cardsArea;
    public List<int> cardsInDeck = new List<int>();
    void Start()
    {
        i = this;
        foreach (var card in AllCardsInGame.i.allCards)
        {
            var newCard = Instantiate(cardPrefab, cardsArea.transform);
            newCard.UpdateStats(card);
        }
    }
    internal void AddCardToDeck(DeckManagerCard deckManagerCard)
    {
        if (cardsInDeck.Count < 19)
        {
            cardsInDeck.Add(deckManagerCard.id);
            var newCard = Instantiate(deckManagerCard, deckArea.transform);
            newCard.isInDeck = true;
            //deckManagerCard.transform.SetParent(deckArea.gameObject.transform);
            //deckManagerCard.isInDeck = true;
        }
    }
    internal void RemoveCardFromDeck(DeckManagerCard deckManagerCard)
    {
        if (cardsInDeck.Count > 0)
        {
            cardsInDeck.Remove(deckManagerCard.id);
            Destroy(deckManagerCard.gameObject);
            //deckManagerCard.transform.SetParent(cardsArea.gameObject.transform);
            //deckManagerCard.isInDeck = false;
        }
    }
}
