using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Assets.Scripts;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager i;
    private List<CardStats> deck;
    public GameCard defaultCard;
	private int maxCredits = 1;
	private int currentCredits = 1;
	public Transform[] cardSlots;
	public GameCard[] availableCardSlots;
	internal GameCard[,] gameBoard = new GameCard[3,8];
    public List<CardPlacement> placements = new List<CardPlacement>();
	public Base[] enemyBases = new Base[3];
	public Base[] teamBases = new Base[3];
    private CardPlacement currentPlacement;
    public GameCard selectedCard;
	public GameObject gameOverPopup;
	public Text gameOverText;
	public TextMeshProUGUI lastTurn;
	public TextMeshProUGUI costText;
	public TextMeshProUGUI turnText;
	public LineRenderer line;
    private bool endingTurn = false;
    public bool gameOver = false;
    public int CurrentCredits { get => currentCredits; set => currentCredits = value; }

    private void Start()
	{
		i = this;
        availableCardSlots = new GameCard[cardSlots.Length];
        GetPlayerDeck();
        DrawCard();
		DrawCard();
		DrawCard();
        UpdateCredits();
    }

    internal void setHighlightedPlacement(CardPlacement placement)
	{
		currentPlacement = placement;
    }
	internal CardPlacement getHighlightedPlacement()
	{
		return currentPlacement;
    }
	internal void DrawCard()
	{
		if (deck.Count > 0)
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
                    deck.Remove(randomCard);
					return;
				}
			}
		}
	}
	public List<GameCard> GetActiveCards() {
        List<GameCard> cardWithAction = new List<GameCard>();
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
        if (!endingTurn)
        {
            endingTurn = true;
            StartCoroutine(StartPhases());
        }
    }

    private IEnumerator StartPhases()
    {
        //Play CPU's cards 
        yield return StartCoroutine(TakeCPUTurn());
        bool attackLoop = true;
        List<GameCard> activeCards = GetActiveCards();
        //check for on arrival abilities
        foreach (var activeCard in activeCards.Where(x=>!x.hasCheckedForArrivalAbilities))
        {
            activeCard.CheckForOnArrivalAbilties();
        }
        //after tur 8 loop forever till game ends
        while (attackLoop)
        {
            //attack
            bool stillAttacking = false;
            activeCards = GetActiveCards();
            foreach (var attacker in activeCards.OrderByDescending(x => x.speed))
            {
                if (!attacker.hasSlowStart && attacker.CurrentHp > 0)
                {
                    var targets = getTargets(attacker);
                    foreach (var target in targets)
                    {
                        stillAttacking = true;
                        var attack = getAttackValue(target, attacker);
                        var laserVariance = attacker.isTeam ? .8f : -.8f;
                        line.SetPosition(0, new Vector3(attacker.transform.position.x, attacker.transform.position.y + 1, attacker.transform.position.z + laserVariance));
                        line.SetPosition(1, new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z));
                        attacker.inAction.SetActive(true);
                        yield return new WaitForSeconds(.1f);
                        target.beingAttacked.SetActive(true);
                        line.gameObject.SetActive(true);
                        yield return new WaitForSeconds(.25f);
                        line.gameObject.SetActive(false);
                        yield return new WaitForSeconds(.1f);
                        attacker.inAction.SetActive(false);
                        target.beingAttacked.SetActive(false);
                        target.UpdateHp(attack);
                        attacker.CheckForOnAttackAbilities(attack, target is GameCard ? target as GameCard: null);
                        yield return new WaitForSeconds(.25f);
                    }
                }
            }

            //check if you won
            if (enemyBases.Count(x => x.CurrentHp > 0) < 2)
            {
                gameOverText.text = "Game Over \n \n You won!";
                gameOverPopup.SetActive(true);
                attackLoop = false;
                gameOver = true;
            }

            //check if enemy won
            if (teamBases.Count(x => x.CurrentHp > 0) < 2)
            {
                    gameOverText.text = "Game Over \n \n You Lost!";
                gameOverPopup.SetActive(true);
                attackLoop = false;
                gameOver = true;
            }

           
            if (maxCredits < 8)
            {
                attackLoop = false;
            }
            else
            {
                lastTurn.text = "Automating!";
                //If no targets are found, exit loop and end in tie
                if (stillAttacking == false)
                {
                    gameOverText.text = "Game Over \n \n Tie!";
                    gameOverPopup.SetActive(true);
                    attackLoop = false;
                    gameOver = true;
                }
            }
        }
        if (!gameOver)
        {
            //reset for next turn
            DrawCard();
            if (maxCredits < 8)
                maxCredits++;
            currentCredits = maxCredits;
            UpdateCredits();
            //re-get to avoid referencing dead enemies
            activeCards = GetActiveCards();
            foreach (var card in activeCards)
            {
                card.hasSlowStart = false;
                card.slow.SetActive(false);
            }
        }
        endingTurn = false;
    }

    private int getAttackValue(Target enemy, GameCard card)
    {
        var attack = card.attack;
        line.material.color = Color.magenta;
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
        if (rand == 0 || (card.doubleMisfire && rand == 1))
        {
            line.material.color = Color.red;
            attack = (int)(attack * .5);
        }
        else if (rand == 18 || rand == 19 || (card.doubleCrit && (rand == 17 || rand == 16)))
        {
            line.material.color = Color.green;
            attack = (int)(attack * 1.25);
        }
        return attack;
    }

    private IEnumerator TakeCPUTurn()
    {
        var aiMana = maxCredits;
        var spawnCount = 0;
        while (aiMana > 0)
        {
            var randomCard = AllCardsInGame.i.allCards[Random.Range(0, AllCardsInGame.i.allCards.Where(x=>x.cost <= aiMana).Count())];
           
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

    private List<Target> getTargets(GameCard attacker)
    {
        //God please forgive me for these 'for' and 'if' statements
        var targetsToReturn = new List<Target>();
        var hasSkippedAttack = false;
        var rowToTarget = attacker.x;
        if (attacker.targetLeft) {
            rowToTarget = 0;
        } 
        else if (attacker.targetRight) {
            rowToTarget = 2;
        }
        else if (attacker.targetCenter) {
            rowToTarget = 1;
        }
        if (!attacker.targetStation)
        {
            for (int i = attacker.isTeam ? 4 :3; attacker.isTeam ? i <= 7 : i >= 0; i = i + (attacker.isTeam ? 1:-1))
            {
                
                if (gameBoard[rowToTarget, i] != null)
                {
                    if (attacker.targetColumn)
                    {
                        targetsToReturn.Add(gameBoard[rowToTarget, i]);
                    }
                    else if (!gameBoard[rowToTarget, i].hasStealth)
                    {
                        if (attacker.targetSkipFirst && !hasSkippedAttack)
                        {
                            hasSkippedAttack = true;
                        }
                        else
                        {
                            if (attacker.targetRow)
                            {
                                for (int column = 0; column < 3; column++)
                                {
                                    if (gameBoard[column, i] != null)
                                    {
                                        targetsToReturn.Add(gameBoard[column, i]);
                                    }
                                }
                                return targetsToReturn;
                            } else {
                                targetsToReturn.Add(gameBoard[rowToTarget, i]);
                            }


                            if (attacker.targetConsecutive)
                            {
                                //check if at end of loop
                                if ((attacker.isTeam && i == 7) || (!attacker.isTeam && i == 0))
                                {
                                    //leave loop so that it adds the station
                                    break;
                                }
                                //check for the next one, add it, and return
                                else if (gameBoard[rowToTarget, i + (attacker.isTeam ? 1 : -1)] != null)
                                {
                                    targetsToReturn.Add(gameBoard[rowToTarget, i + (attacker.isTeam ? 1 : -1)]);
                                    return targetsToReturn;
                                }
                            } else if (attacker.targetLastInColumn) {
                                //collecting all targets and determining last at end 
                            }
                            else
                            {
                                return targetsToReturn;
                            }
                        }
                    }
                }
            }
        }
        if (attacker.targetLastInColumn && targetsToReturn.Count > 0)
        {
            return new List<Target>() { targetsToReturn.LastOrDefault() };
        }
        if (attacker.targetRow)
        {
            for (int column = 0; column < 3; column++)
            {
                if ((attacker.isTeam ? enemyBases[column].CurrentHp : teamBases[column].CurrentHp) > 0)
                {
                    targetsToReturn.Add(attacker.isTeam ? enemyBases[column] : teamBases[column]);
                }
            }
        }
        else if ((attacker.isTeam ? enemyBases[rowToTarget].CurrentHp: teamBases[rowToTarget].CurrentHp) > 0)
        {
            targetsToReturn.Add(attacker.isTeam ? enemyBases[rowToTarget] : teamBases[rowToTarget]);
        }
        return targetsToReturn;
    }

    public void UpdateCredits(int cost = 0)
	{
        currentCredits -= cost;
        costText.text = $"{currentCredits}{(currentCredits==0?"":"M")} Credits";
        turnText.text = $"Turn {maxCredits}/8";
        if (maxCredits == 8)
        {
            lastTurn.gameObject.SetActive(true);
        }
        foreach (var cardInHand in availableCardSlots.Where(x => x != null))
        {
			if (cardInHand.cost > currentCredits)
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
        deck = new List<CardStats>();
        if (PlayerDeck.i != null)
        {
            foreach (var cardId in PlayerDeck.i.cardsInDeck)
            {
                deck.Add(AllCardsInGame.i.allCards.FirstOrDefault(x => x.id == cardId));
            }
        }
        else
        {
            deck = AllCardsInGame.i.allCards;
        }
    }

    internal void ReorganizeHand()
    {
        List<GameCard> cardsInHand = availableCardSlots.Where(x => x != null).ToList();
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
