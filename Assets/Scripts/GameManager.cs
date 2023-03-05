using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager i;
	public List<Card> deck;
	// public TextMeshProUGUI deckSizeText;

	public Transform[] cardSlots;
	public Card[] availableCardSlots = new Card[5];

    private Vector3? currentPlacement;
    public List<Card> discardPile;
	// public TextMeshProUGUI discardPileSizeText;

	private Animator camAnim;

	private void Start()
	{
		i = this;
        // camAnim = Camera.main.GetComponent<Animator>();
    }
	internal void setHighlightedPlacement(Vector3? placement)
	{
		currentPlacement = placement;
    }
	internal Vector3? getHighlightedPlacement()
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

	private void Update()
	{
		// deckSizeText.text = deck.Count.ToString();
		// discardPileSizeText.text = discardPile.Count.ToString();
	}

}
