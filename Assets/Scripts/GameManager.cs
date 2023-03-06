using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
	public static GameManager i;
	public List<Card> deck;

	private int maxMana = 1;
	private int currentMana = 1;
	public Transform[] cardSlots;
	public Card[] availableCardSlots;
	public Card[,] gameBoard = new Card[3,6];
    public List<CardPlacement> placements = new List<CardPlacement>();
	public Target[] enemyBases = new Target[3];
	public Target[] teamBases = new Target[3];
    private CardPlacement currentPlacement;
    public Card selectedCard;
	public GameObject popup;
	public TextMeshProUGUI manaText;
	public LineRenderer line;
    private bool endingTurn = false;


    private Animator camAnim;

    public int CurrentMana { get => currentMana; set => currentMana = value; }

    private void Start()
	{
		i = this;
		availableCardSlots = new Card[cardSlots.Length];

        DrawCard();
		DrawCard();
		DrawCard();
        UpdateMana();

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
	private void DrawCard()
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
                    var newCard = Instantiate(randomCard, cardSlots[i].transform.position, cardSlots[i].rotation);
                    newCard.isTeam = true;
                    newCard.handIndex = i;
                    availableCardSlots[i] = newCard;
                    //deck.Remove(randomCard);
					return;
				}
			}
		}
	}
	public List<Card> GetActiveCards() {
        List<Card> cardWithAction = new List<Card>();
        //check all played cards
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                var currentCard = gameBoard[i, j];
                if (currentCard != null)
                {
                    cardWithAction.Add(currentCard);
                }
            }
        }
		return cardWithAction;
    }
	public void EndTurn()
	{
        if (!endingTurn) {
            endingTurn = true;
            StartCoroutine(StartPhases());
        }
    }

    private IEnumerator StartPhases()
    {
        //Play CPU's cards 
        yield return StartCoroutine(TakeCPUTurn());

        //attack
        List<Card> activeCards = GetActiveCards();
        foreach (var card in activeCards.OrderByDescending(x => x.speed))
        {
            if (!card.hasSummonSickness && card.CurrentHp > 0 )
            {
                var enemy = getTarget(card);
                line.SetPosition(0, card.transform.position);
                line.SetPosition(1, enemy.transform.position);
                card.inAction.SetActive(true);
                yield return new WaitForSeconds(.5f);
                line.gameObject.SetActive(true);
                yield return new WaitForSeconds(.25f);
                card.inAction.SetActive(false);
                line.gameObject.SetActive(false);
                enemy.UpdateHp(getAttackValue(enemy, card));
                yield return new WaitForSeconds(.25f);
            }
        }

        //check if you won
        if (enemyBases.Count(x => x.CurrentHp > 0) < 2)
        {
            popup.SetActive(true);
        }

        //check if enemy won
        if (teamBases.Count(x => x.CurrentHp > 0) < 2)
        {
            popup.SetActive(true);
        }

        //reset for next turn
        DrawCard();
        if (maxMana < 8)
            maxMana++;
        currentMana = maxMana;
        UpdateMana();
        //reget to avoid referencing dead enemies
        activeCards = GetActiveCards();
        foreach (var card in activeCards)
        {
            card.hasSummonSickness = false;
            card.disabled.SetActive(false);
        }
        endingTurn = false;
    }

    private int getAttackValue(Target enemy, Card card)
    {
        var attack = card.attack;
        //check affinities
        if ((card.affinity == 1 && enemy.affinity == 2) || (card.affinity == 2 && enemy.affinity == 3) || (card.affinity == 3 && enemy.affinity == 1))
        {
            attack = (int)(attack * 1.25);
        }
        else if ((card.affinity == 2 && enemy.affinity == 1) || (card.affinity == 3 && enemy.affinity == 2) || (card.affinity == 1 && enemy.affinity == 3))
        {
            attack = (int)(attack * .75);
        }
        //check for crits and misfires
        var rand = Random.Range(0, 20);
        if (rand == 0)
        {
            attack = (int)(attack * .5);
        }
        else if (rand == 18 || rand == 19)
        {
            attack = (int)(attack * 1.25);
        }
        return attack;
    }

    private IEnumerator TakeCPUTurn()
    {
        Card randomCard = deck[Random.Range(0, deck.Count)];
        var newCard = Instantiate(randomCard);
        newCard.isTeam = false;
        newCard.AIPlayCard();
        yield return new WaitForSeconds(.25f);
    }

    private Target getTarget(Card card)
    {
        bool hasSkipAttack = card.attackType == 1;
        if (card.y < 3)
        {
            for (int i = 3; i < 6; i++)
            {
                if (gameBoard[card.x, i] != null)
                {
                    if (hasSkipAttack)
                    {
                        hasSkipAttack = false;
                    }
                    else
                    {
                        return gameBoard[card.x, i];
                    }
                }
            }
            return enemyBases[card.x];
        }
        else
        {
            for (int i = 2; i >= 0; i--)
            {
                if (gameBoard[card.x, i] != null)
                {
                    if (hasSkipAttack)
                    {
                        hasSkipAttack = false;
                    }
                    else
                    {
                        return gameBoard[card.x, i];
                    }
                }
            }
            return teamBases[card.x];
        }
    }

    public void UpdateMana(int cost = 0)
	{
        currentMana -= cost;
        manaText.text = $"Mana: {currentMana}";
        foreach (var cardInHand in availableCardSlots.Where(x => x != null))
        {
			if (cardInHand.cost > currentMana)
			{
				cardInHand.disabled.SetActive(true);
			}
			else
			{
                cardInHand.disabled.SetActive(false);
            }
        }
    }

}
