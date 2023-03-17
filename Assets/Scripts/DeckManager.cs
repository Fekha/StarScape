using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public static DeckManager i;
    public DeckManagerCard cardPrefab;
    public GameObject deckArea;
    public GameObject cardsArea;
    public Button playButton;
    public TextMeshProUGUI cardsInDeckText;
    private int deckSize = 15;
    void Start()
    {
        i = this;
        foreach (var card in AllCardsInGame.i.allCards)
        {
            var newCard = Instantiate(cardPrefab, cardsArea.transform);
            newCard.transform.localScale = new Vector3(.65f, .65f, .65f);
            newCard.UpdateStats(card);
        }
    }
    internal void AddCardToDeck(DeckManagerCard deckManagerCard)
    {
        if (PlayerDeck.i.cardsInDeck.Count < deckSize)
        {
            PlayerDeck.i.cardsInDeck.Add(deckManagerCard.id);
            var newCard = Instantiate(deckManagerCard, deckArea.transform);
            newCard.transform.localScale = new Vector3(.5f, .5f, .5f);
            newCard.isInDeck = true;
            cardsInDeckText.text = $"Build a deck! {PlayerDeck.i.cardsInDeck.Count}/{deckSize}";

            //deckManagerCard.transform.SetParent(deckArea.gameObject.transform);
            //deckManagerCard.isInDeck = true;
        }
        if (PlayerDeck.i.cardsInDeck.Count == deckSize)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }
    internal void RemoveCardFromDeck(DeckManagerCard deckManagerCard)
    {
        if (PlayerDeck.i.cardsInDeck.Count > 0)
        {
            playButton.interactable = false;
            PlayerDeck.i.cardsInDeck.Remove(deckManagerCard.id);
            Destroy(deckManagerCard.gameObject);
            cardsInDeckText.text = $"Build a deck! {PlayerDeck.i.cardsInDeck.Count}/{deckSize}";
            //deckManagerCard.transform.SetParent(cardsArea.gameObject.transform);
            //deckManagerCard.isInDeck = false;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
