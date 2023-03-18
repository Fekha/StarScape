using Assets.Scripts;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameCard : Target
{
    //Card Attack Patterns
    internal bool targetConsecutive = false;
    internal bool targetColumn = false;
    internal bool targetSkipFirst = false;
    internal bool targetLastInColumn = false;
    internal bool targetStation = false;
    internal bool targetLeft;
    internal bool targetRight;
    internal bool targetCenter;
    internal bool targetRow;

    //Card Abilities
    internal bool hasSlowStart = false;
    internal bool hasStealth = false;
    internal bool onAttackGain1Attack = false;
    internal bool onAttackHeal2 = false;
    internal bool reflect20percent = false;
    internal bool doubleCrit = false;
    internal bool onAttackGain2Attack = false;
    internal bool onArrivalDrawCard = false;
    internal bool doubleMisfire = false;
    internal bool onAttackDeal1DamageToSelf = false;
    internal bool onAttackDeal2DamageToSelf = false;
    internal bool onAttackDeal4DamageToSelf = false;
    internal bool onArrivalHealAllies3 = false;

    //Game variables
    public bool isSelected;
    public bool isTeam;
    public bool isViewMode;
    public int handIndex;
    public bool hasBeenPlayed = false;
    public Vector3 originPos;
    public Quaternion originRot;

    //Game Card visuals
    public GameCard viewableCard;
    public GameObject disabled;
    public GameObject slow;
    public GameObject stealthed;
    public GameObject inAction;
    private bool onArrivalDamageEnemies2;

    public bool OnAttackLose1Attak { get; private set; }

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
                PlayCard(currentPlacement);
                GameManager.i.availableCardSlots[handIndex] = null;
                GameManager.i.UpdateCredits(cost);
                GameManager.i.ReorganizeHand();
            }
        }
    }

    private void PlayCard(CardPlacement placement)
    {
        if (hasSlowStart && !isViewMode)
            slow.SetActive(true);

        if (hasStealth && !isViewMode)
            stealthed.SetActive(true);

        disabled.SetActive(false);
        HandBorder.SetActive(false);

        GameManager.i.CardsPlayedThisTurn.Add(this);
        hasBeenPlayed = true;
        x = placement.x;
        y = placement.y;
        GameManager.i.gameBoard[x, y] = this;
        transform.position = new Vector3(placement.transform.position.x, placement.transform.position.y, placement.transform.position.z - .2f);
        transform.localScale = new Vector3(.75f, .8f, 1);
        var boxCollider = GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(0.002483845f, 0.2560225f, -0.03949931f);
        boxCollider.size = new Vector3(2.575973f, 2.491651f, 0.2789987f);
    }

    //combine logic later with onmouseup
    public void AIPlayCard(int cardX, int cardY)
    {
        x = cardX; y = cardY;
        var placement = GameManager.i.placements.FirstOrDefault(c => c.x == cardX && c.y == cardY);
        PlayCard(placement);
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
                viewableCard = ViewCard();
            }
            else
            {

                if (!isSelected)
                {
                    if (GameManager.i.CurrentCredits >= cost)
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
        if (!isViewMode && !isSelected && !hasBeenPlayed && viewableCard == null && GameManager.i.selectedCard == null)
        {
            viewableCard = ViewCard();
        }
    }
    public void OnMouseExit()
    {
        if (!hasBeenPlayed && viewableCard != null)
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
        setAbility(stat.id);
        setAttackTarget(stat.targetId);
    }

    private void setAttackTarget(int attackPattern)
    {
        switch (attackPattern) {
            case 1: targetSkipFirst = true; break;
            case 2: targetLastInColumn = true; break;
            case 3: targetStation = true; break;
            case 4: targetConsecutive = true; break;
            case 5: targetColumn = true; break;
            case 6: targetLeft = true; break;
            case 7: targetRight = true; break;
            case 8: targetCenter = true; break;
            case 9: targetRow = true; break;
        }
    }

    private void setAbility(int ability)
    {
        switch (ability)
        {
            case 0: break;
            case 1: onAttackGain1Attack = true; break;
            case 2: hasSlowStart = true; break;
            case 3: onAttackDeal1DamageToSelf = true; break;
            case 4: hasStealth = true; break;
            case 5: doubleMisfire = true; break; 
            case 6: onArrivalHealAllies3 = true; break;
            case 7: onAttackDeal2DamageToSelf = true; break;
            case 8: onArrivalDamageEnemies2 = true; break;
            case 9: onArrivalDrawCard = true; break;
            case 10: break;
            case 11: onAttackGain2Attack = true; break;
            case 12: OnAttackLose1Attak = true; break;
            case 13: break;
            case 14: hasSlowStart = true; break;
            case 15: doubleCrit = true; break;
            case 16: reflect20percent = true; break;
            case 17: break;
            case 18: onAttackHeal2 = true; break;
            case 19: break;
            case 20: onAttackDeal4DamageToSelf = true; break;
            default: break;
        }
    }

    public void Die()
    {
        GameManager.i.gameBoard[x, y] = null;
        Destroy(this.gameObject);
    }

    public override void UpdateHp(int attack)
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
    public void CheckForOnAttackAbilities(int attack, GameCard target = null)
    {
        if (onAttackDeal1DamageToSelf)
        {
            UpdateHp(1);
        } 
        if (onAttackDeal2DamageToSelf)
        {
            UpdateHp(2);
        } 
        if (onAttackDeal4DamageToSelf)
        {
            UpdateHp(4);
        }
        if (OnAttackLose1Attak)
        {
            if(this.attack > 1)
                this.attack -= 1;
            attackText.text = $"{this.attack}";
        }  
        if (onAttackGain1Attack)
        {
            this.attack += 1;
            attackText.text = $"{this.attack}";
        } 
        if (onAttackGain2Attack)
        {
            this.attack += 2;
            attackText.text = $"{this.attack}";
        }
        if (onAttackHeal2)
        {
            UpdateHp(-2);
        }
        if (target != null)
        {
            if (target.reflect20percent)
            {
                UpdateHp((int)(attack * .2));
            }
        }
    }
    internal void CheckForOnArrivalAbilties()
    {
        if (onArrivalDrawCard && isTeam)
        {
            GameManager.i.DrawCard();
        }
        if (onArrivalHealAllies3)
        {
            var teammates = GameManager.i.GetActiveCards().Where(x=>x.isTeam == isTeam);
            foreach (var mate in teammates)
            {
                mate.UpdateHp(-3);
            }
        }
        if (onArrivalDamageEnemies2)
        {
            var enemies = GameManager.i.GetActiveCards().Where(x => x.isTeam != isTeam);
            foreach (var enemy in enemies)
            {
                enemy.UpdateHp(2);
            }
        }
    }
    private GameCard ViewCard()
    {
        GameCard infoCard = Instantiate(this, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y -1, Camera.main.transform.position.z + 3), Camera.main.transform.rotation);
        //infoCard.CostIcon.SetActive(true);
        //infoCard.SpeedIcon.SetActive(false);
        infoCard.HandBorder.SetActive(true);
        infoCard.isViewMode = true;
        infoCard.stealthed.SetActive(false);
        infoCard.slow.SetActive(false);
        infoCard.inAction.SetActive(false);
        infoCard.beingAttacked.SetActive(false);
        foreach (var child in infoCard.GetComponentsInChildren<SpriteRenderer>())
        {
            child.sortingLayerName = "Popup";
        }
        foreach (var child in infoCard.GetComponentsInChildren<TextMeshPro>())
        {
            child.sortingLayerID = SortingLayer.NameToID("Popup");
        }
        infoCard.transform.localScale += new Vector3(3, 3, 3);
        return infoCard;
    }

    internal void SetSpeed(int newSpeed)
    {
        CostIcon.SetActive(false);
        SpeedIcon.SetActive(true);
        speed = newSpeed;
        speedText.text = $"{newSpeed}";
    }
}
