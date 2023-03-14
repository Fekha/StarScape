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
    public GameCard defaultCard;
	private int maxMana = 1;
	private int currentMana = 1;
	public Transform[] cardSlots;
	public GameCard[] availableCardSlots;
	internal GameCard[,] gameBoard = new GameCard[3,8];
    public List<CardPlacement> placements = new List<CardPlacement>();
	public Base[] enemyBases = new Base[3];
	public Base[] teamBases = new Base[3];
    private CardPlacement currentPlacement;
    public GameCard selectedCard;
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
		availableCardSlots = new GameCard[cardSlots.Length];
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
        List<GameCard> activeCards = GetActiveCards();
        foreach (var attacker in activeCards.OrderByDescending(x => x.speed))
        {
            if (!attacker.hasSummonSickness && attacker.CurrentHp > 0)
            {
                var targets = getTargets(attacker);
                foreach(var target in targets) {
                    var attack = getAttackValue(target, attacker);
                    var laserVariance = attacker.isTeam ? .9f : -.9f;
                    line.SetPosition(0, new Vector3(attacker.transform.position.x, attacker.transform.position.y + 1, attacker.transform.position.z + laserVariance));
                    line.SetPosition(1, new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z));
                    attacker.inAction.SetActive(true);
                    yield return new WaitForSeconds(.25f);
                    target.beingAttacked.SetActive(true);
                    line.gameObject.SetActive(true);
                    yield return new WaitForSeconds(.25f);
                    line.gameObject.SetActive(false);
                    yield return new WaitForSeconds(.25f);
                    attacker.inAction.SetActive(false);
                    target.beingAttacked.SetActive(false);
                    target.UpdateHp(attack);
                    attacker.CheckForAbilities(attack);
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
        //re-get to avoid referencing dead enemies
        activeCards = GetActiveCards();
        foreach (var card in activeCards)
        {
            card.hasSummonSickness = false;
            card.slow.SetActive(false);
        }
        endingTurn = false;
    }

    private int getAttackValue(Target enemy, GameCard card)
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

    private List<Target> getTargets(GameCard attacker)
    {
        //God please forgive me for these 'for' and 'if' statements
        var targetsToReturn = new List<Target>();
        var hasSkippedAttack = false;
        if (!attacker.attackOnlyStation)
        {
            for (int i = attacker.isTeam ? 4 :3; attacker.isTeam ? i <= 7 : i >= 0; i = i + (attacker.isTeam ? 1:-1))
            {
                if (gameBoard[attacker.x, i] != null)
                {
                    if (attacker.attackWholeColumn)
                    {
                        targetsToReturn.Add(gameBoard[attacker.x, i]);
                    }
                    else if (!gameBoard[attacker.x, i].hasStealth)
                    {
                        if (attacker.attackSkipFirst && !hasSkippedAttack)
                        {
                            hasSkippedAttack = true;
                        }
                        else
                        {
                            targetsToReturn.Add(gameBoard[attacker.x, i]);
                            if (attacker.attackConsecutive1)
                            {
                                //check if at end of loop
                                if ((attacker.isTeam && i == 7) || (!attacker.isTeam && i == 0))
                                {
                                    //leave loop so that it adds the station
                                    break;
                                }
                                //check for the next one, add it, and return
                                else if (gameBoard[attacker.x, i + (attacker.isTeam ? 1 : -1)] != null)
                                {
                                    targetsToReturn.Add(gameBoard[attacker.x, i + (attacker.isTeam ? 1 : -1)]);
                                    return targetsToReturn;
                                }
                            } else if (attacker.attackLastInColumn) {
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
        if (attacker.attackLastInColumn && targetsToReturn.Count > 0)
        {
            return new List<Target>() { targetsToReturn.LastOrDefault() };
        }
        if ((attacker.isTeam ? enemyBases[attacker.x].CurrentHp: teamBases[attacker.x].CurrentHp) > 0)
        {
            targetsToReturn.Add(attacker.isTeam ? enemyBases[attacker.x] : teamBases[attacker.x]);
        }
        return targetsToReturn;
    }

    public void UpdateMana(int cost = 0)
	{
        currentMana -= cost;
        manaText.text = $"{currentMana}{(currentMana==0?"":"M")} Credits";
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
        deck = AllCardsInGame.i.allCards;
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
