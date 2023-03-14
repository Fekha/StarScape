using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameCard : Target
{
    //Card Attack Patterns
    public bool attackConsecutive1 = false;
    public bool attackWholeColumn = false;
    public bool attackSkipFirst = false;
    public bool attackLastInColumn = false;
    public bool attackOnlyStation = false;

    //Card Abilities
    public bool hasSummonSickness = false;
    public bool hasStealth = false;
    public int burnout = 0;
    public double drain = 0;
    public int scavenger = 0;

    //Game variables
    public bool isSelected;
    public bool isTeam;
    public bool isViewMode;
    public int handIndex;
    public bool hasBeenPlayed;
    public Vector3 originPos;
    public Quaternion originRot;

    //Game Card visuals
    public GameCard viewableCard;
    public GameObject disabled;
    public GameObject slow;
    public GameObject stealthed;
    public GameObject inAction;
    public TextMeshPro abilityTextObj;
    public GameObject cardText;
    public TextMeshPro attackTextObj;


    public void Update()
    {
        if (isSelected)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            var newPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = newPos;
        }
    }
    private void OnMouseUp()
	{
        if (isSelected)
        {
            isSelected = false;
            GameManager.i.selectedCard = null;
            var currentPlacement = GameManager.i.getHighlightedPlacement();
            if (currentPlacement == null)
			{
                transform.position = originPos;
                transform.rotation = originRot;
            }
			else
			{
                if (hasSummonSickness)
                    slow.SetActive(true);

                if (hasStealth)
                    stealthed.SetActive(true);

                disabled.SetActive(false);
                cardText.SetActive(false);
                GameManager.i.availableCardSlots[handIndex] = null;
                GameManager.i.UpdateMana(cost);
                hasBeenPlayed = true;
                x = currentPlacement.x;
                y = currentPlacement.y;
                GameManager.i.gameBoard[x,y] = this;
                transform.position = new Vector3(currentPlacement.transform.position.x, currentPlacement.transform.position.y+.1f, currentPlacement.transform.position.z);
                GameManager.i.ReorganizeHand();
            }
        }
    }
    //combine logic later with onmouseup
    public void AIPlayCard(int cardX, int cardY)
    {
        x = cardX; y = cardY;
        if (hasSummonSickness)
            slow.SetActive(true);

        if (hasStealth)
            stealthed.SetActive(true);

        disabled.SetActive(false);
        cardText.SetActive(false);
        hasBeenPlayed = true;
        GameManager.i.gameBoard[cardX, cardY] = this;
        var placement = GameManager.i.placements.FirstOrDefault(c => c.x == cardX && c.y == cardY);
        transform.position = new Vector3(placement.transform.position.x, placement.transform.position.y + .1f, placement.transform.position.z);

    }
    private void OnMouseDown()
    {
        if (isViewMode)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (hasBeenPlayed && viewableCard == null)
            {
                ViewCard();
            }
            else
            {

                if (!isSelected)
                {
                    if (GameManager.i.CurrentMana >= cost)
                    {
                        originPos = transform.position;
                        originRot = transform.rotation;
                        GameManager.i.selectedCard = this;
                        isSelected = true;
                        transform.rotation = Camera.main.transform.rotation;
                    }
                }
                //Instantiate(hollowCircle, transform.position, Quaternion.identity);
                // camAnim.SetTrigger("shake");

            }
        }
	}

    public void OnMouseEnter()
    {
        if (!isViewMode && !isSelected && !hasBeenPlayed && GameManager.i.selectedCard == null)
        {
            viewableCard = ViewCard();
        }
    }
    public void OnMouseExit()
    {
        if (viewableCard != null)
        {
            Destroy(viewableCard.gameObject);
            viewableCard = null;
        }
        if (isViewMode)
        {
            Destroy(this.gameObject);
        }
    }

    internal override void UpdateStats(CardStats stat)
    {
        base.UpdateStats(stat);
        setAbilities(stat.abilities);
        setAttackTarget(stat.targetId);
    }

    private void setAttackTarget(int attackPattern)
    {
        switch (attackPattern) {
            case 1: attackSkipFirst = true; break;
            case 2: attackLastInColumn = true; break;
            case 3: attackOnlyStation = true; break;
            case 4: attackConsecutive1 = true; break;
            case 5: attackWholeColumn = true; break;
        }
    }

    private void setAbilities(List<int> abilities)
    {
        if (abilities.Contains(1))
        {
            hasSummonSickness = true;
        }
        if (abilities.Contains(2))
        {
            burnout = 2;
        }
        if (abilities.Contains(3))
        {
            hasStealth = true;
        }
        if (abilities.Contains(4))
        {
            drain = -.1;
        }
        if (abilities.Contains(5))
        {
            scavenger = 2;
        }
    }

    public void Die()
    {
        GameManager.i.gameBoard[x, y] = null;
        Destroy(this.gameObject);
    }

    public void UpdateHp(int attack)
    {
        CurrentHp -= attack;
        if (CurrentHp > maxHp)
        {
            CurrentHp = maxHp;
        }
        hpText.text = $"{CurrentHp}";
        if (CurrentHp <= 0)
        {
            Die();
        }
    }
    public void CheckForAbilities(int attack)
    {
        UpdateHp(burnout);
        UpdateHp((int)(attack * drain));
        this.attack += scavenger;
        attackText.text = $"{this.attack}";
    }
    private GameCard ViewCard()
    {
        GameCard infoCard = Instantiate(this, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 1, Camera.main.transform.position.z + 3), Camera.main.transform.rotation);
        infoCard.cardText.SetActive(true);
        infoCard.isViewMode = true;
        foreach(var child in infoCard.GetComponentsInChildren<SpriteRenderer>()) {
            child.sortingLayerName = "Popup";
        }
        foreach (var child in infoCard.GetComponentsInChildren<TextMeshPro>())
        {
            child.sortingLayerID = SortingLayer.NameToID("Popup");
        }
        infoCard.transform.localScale += new Vector3(3, 3, 3);
        return infoCard;
    }
}
