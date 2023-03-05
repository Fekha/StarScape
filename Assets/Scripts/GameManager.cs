using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public static GameManager i;
	public List<Card> deck;
	// public TextMeshProUGUI deckSizeText;

	public Transform[] cardSlots;
	public Card[] availableCardSlots = new Card[5];
	public Card[,] gameBoard = new Card[3,6];
	public Base[] enemyBases = new Base[3];
    private CardPlacement currentPlacement;
    public List<Card> discardPile;
	public GameObject popup;
	// public TextMeshProUGUI discardPileSizeText;

	private Animator camAnim;

	private void Start()
	{
		i = this;
		DrawCard();
		DrawCard();
		DrawCard();
        // camAnim = Camera.main.GetComponent<Animator>();
    }
	internal void setHighlightedPlacement(CardPlacement placement)
	{
		currentPlacement = placement;
    }
	internal CardPlacement getHighlightedPlacement()
	{
		return currentPlacement;
    }
	public void DrawCard()
	{
		if (deck.Count >= 1)
		{
			// camAnim.SetTrigger("shake");
			//This hand and deck system with both be redone
			Card randomCard = deck[Random.Range(0, deck.Count)];
			for (int i = 0; i < availableCardSlots.Length; i++)
			{
				if (availableCardSlots[i] == null)
				{
                    var newCard = Instantiate(randomCard, cardSlots[i].position, cardSlots[i].rotation);
                    newCard.handIndex = i;
                    availableCardSlots[i] = newCard;
                    deck.Remove(randomCard);
					return;
				}
			}
		}
	}

	public void Shuffle()
	{
		if (discardPile.Count >= 1)
		{
			foreach (Card card in discardPile)
			{
				deck.Add(card);
			}
			discardPile.Clear();
		}
	}

	public void EndTurn()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				var currentCard = gameBoard[i, j];
				if (currentCard != null)
				{
					Card enemyCard = null;
                    for (int k = 5; k >= 3; k--)
					{
                        enemyCard = gameBoard[i, k];
                    }
					if (enemyCard == null)
					{
                        enemyBases[i].UpdateHp(currentCard.attack);
                    }
                    else
					{
                        enemyCard.UpdateHp(currentCard.attack);
                    }
                }
			}
		}
		//check if you won
		if (enemyBases.Count(x=>x.hp > 0) < 2)
		{
			popup.SetActive(true);
        }
        DrawCard();
    }

}
