using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Assets.Scripts;

public class GameManager : MonoBehaviour
{
	public static GameManager i;
    private List<CardStats> deck;
    public List<Sprite> cardSprites;
    public Card defaultCard;
	private int maxMana = 1;
	private int currentMana = 1;
	public Transform[] cardSlots;
	public Card[] availableCardSlots;
	public Card[,] gameBoard = new Card[3,8];
    public List<CardPlacement> placements = new List<CardPlacement>();
	public Target[] enemyBases = new Target[3];
	public Target[] teamBases = new Target[3];
    private CardPlacement currentPlacement;
    public Card selectedCard;
	public GameObject popup;
	public TextMeshProUGUI manaText;
	public LineRenderer line;
    private bool endingTurn = false;
    public Material laserBaseMaterial;
    public Material laserCritMaterial;
    public Material laserMissMaterial;
    public int CurrentMana { get => currentMana; set => currentMana = value; }

    private void Start()
	{
		i = this;
		availableCardSlots = new Card[cardSlots.Length];
        GetPlayerDeck();
        DrawCard();
		DrawCard();
		DrawCard();
        UpdateMana();
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
			CardStats randomCard = deck[Random.Range(0, deck.Count)];
			for (int i = 0; i < availableCardSlots.Length; i++)
			{
				if (availableCardSlots[i] == null)
				{
                    var newCard = Instantiate(defaultCard, cardSlots[i].transform.position, cardSlots[i].rotation);
                    newCard.UpdateStats(randomCard);
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
            for (int j = 0; j < 8; j++)
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
            if (!card.hasSummonSickness && card.CurrentHp > 0)
            {
                var enemy = getTarget(card);
                if (enemy != null)
                {
                    var attack = getAttackValue(enemy, card);
                    var variance = card.isTeam ? .9f : -.9f;
                    line.SetPosition(0, new Vector3(card.transform.position.x, card.transform.position.y + 1, card.transform.position.z + variance));
                    line.SetPosition(1, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, enemy.transform.position.z));
                    card.inAction.SetActive(true);
                    yield return new WaitForSeconds(.25f);
                    enemy.beingAttacked.SetActive(true);
                    line.gameObject.SetActive(true);
                    yield return new WaitForSeconds(.25f);
                    line.gameObject.SetActive(false);
                    yield return new WaitForSeconds(.25f);
                    card.inAction.SetActive(false);
                    enemy.beingAttacked.SetActive(false);
                    enemy.UpdateHp(attack);
                    yield return new WaitForSeconds(.25f);
                    line.material = laserBaseMaterial;
                }
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
            line.material = laserMissMaterial;
            attack = (int)(attack * .5);
        }
        else if (rand == 18 || rand == 19)
        {
            line.material = laserCritMaterial;
            attack = (int)(attack * 1.25);
        }
        return attack;
    }

    private IEnumerator TakeCPUTurn()
    {
        var aiMana = maxMana;
        var spawnCount = 0;
        while (aiMana > 0)
        {
            var randomCard = deck[Random.Range(0, deck.Where(x=>x.cost <= aiMana).Count())];
           
            var openLanes = teamBases.Where(x => x.CurrentHp > 0).ToList();
            var cardX = openLanes[Random.Range(0, openLanes.Count())].x;
            var cardY = Random.Range(4, 8);
            while (gameBoard[cardX, cardY] != null)
            {
                cardX = openLanes[Random.Range(0, openLanes.Count())].x;
                cardY = Random.Range(4, 8);
                if (gameBoard[cardX, 4] != null && gameBoard[cardX, 5] != null && gameBoard[cardX, 6] != null && gameBoard[cardX, 7] != null)
                {
                    break;
                }
            }
            if (gameBoard[cardX, cardY] == null && spawnCount < 2)
            {
                var newCard = Instantiate(defaultCard);
                newCard.UpdateStats(randomCard);
                newCard.isTeam = false;
                aiMana -= newCard.cost;
                newCard.AIPlayCard(cardX, cardY);
                spawnCount++;
            }
            else
            {
                break;
            }
        }
        yield return new WaitForSeconds(.25f);
    }

    private Target getTarget(Card card)
    {
        bool hasSkipAttack = card.attackPattern == 1;
        if (card.y < 4)
        {
            for (int i = 4; i <= 7; i++)
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
            if (enemyBases[card.x].CurrentHp > 0)
            {
                return enemyBases[card.x];
            }
            return null;
        }
        else
        {
            for (int i = 3; i >= 0; i--)
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
            if (teamBases[card.x].CurrentHp > 0)
            {
                return teamBases[card.x];
            }
            return null;
        }
    }

    public void UpdateMana(int cost = 0)
	{
        currentMana -= cost;
        manaText.text = $"{currentMana}M Credits";
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

    private void GetPlayerDeck()
    {
        deck = new List<CardStats>()
        {
            new CardStats(0,0,1,20,3,3,0,new List<int>() { 1 },"Slow"),
            new CardStats(1,1,1,11,4,5),
            new CardStats(2,2,1,4,6,10,1,null,"Skip"),
            new CardStats(3,3,1,7,7,7),
            new CardStats(4,3,2,24,6,5),
            new CardStats(5,2,2,18,12,6,1,null,"Skip"),
            new CardStats(6,1,2,11,11,11),
            new CardStats(7,0,2,2,25,16,0,new List<int>() { 1 },"Slow"),
            new CardStats(8,0,3,40,2,2),
            new CardStats(9,1,3,25,4,14),
            new CardStats(10,2,3,15,15,15,1,null,"Skip")
        };
    }

    internal void ReorganizeHand()
    {
        List<Card> cardsInHand = availableCardSlots.Where(x => x != null).ToList();
        for (int i = 0; i < availableCardSlots.Length; i++)
        {
            availableCardSlots[i] = null;
        }
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            var card = cardsInHand[i];
            card.handIndex = i;
            card.transform.position = cardSlots[i].transform.position;
            card.transform.rotation = cardSlots[i].transform.rotation;
            availableCardSlots[i] = card;
        }
    }
}
