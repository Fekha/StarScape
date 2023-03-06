using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Card : Target
{
    public int id;
	public bool hasBeenPlayed;
	public bool isSelected;
    public bool isTeam;
    public bool isViewMode;
    public Card viewableCard;
    public bool hasSummonSickness = true;
	public int handIndex;
	public int attackType;
    public TextMeshPro attackText;
    public TextMeshPro costText;
    public TextMeshPro speedText;
    public TextMeshPro abilityText;
    public GameObject disabled;
    public GameObject inAction;
    public GameObject cardText;
    public Vector3 origin;
    public int cost = 1;
    public int attack = 5;
    public double speed = 7;
    public SpriteRenderer affinityColor;
    public Sprite antimatter;
    public Sprite electric;
    public Sprite thermal;
    public Sprite chemical;
    public string ability = "None";
    private void Start()
	{
        setAffinity();
        CurrentHp = maxHp;
        hpText.text = $"{CurrentHp}";
        attackText.text = $"{attack}";
        costText.text = $"{cost}";
        speedText.text = $"{speed}";
        abilityText.text = ability;
    }
    public virtual void setAffinity()
    {
        switch (affinity)
        {
            case (int)Enums.Affinities.Antimatter:
                    affinityColor.sprite = antimatter;
                break;
            case (int)Enums.Affinities.Electrical:
                    affinityColor.sprite = electric;
                break;
            case (int)Enums.Affinities.Thermal:
                    affinityColor.sprite = thermal;
                break;
            case (int)Enums.Affinities.Chemical:
                    affinityColor.sprite = chemical;
                break;
        }
    }
    public void AIPlayCard()
    {
        if (hasSummonSickness)
            disabled.SetActive(true);

        cardText.SetActive(false);
        hasBeenPlayed = true;
        x = Random.Range(0, 3);
        y = Random.Range(3, 6);
        if (GameManager.i.gameBoard[x, y] == null)
        {
            GameManager.i.gameBoard[x, y] = this;
            var placement = GameManager.i.placements.FirstOrDefault(c => c.x == x && c.y == y);
            transform.position = new Vector3(placement.transform.position.x, placement.transform.position.y + .1f, placement.transform.position.z);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public override void Die()
    {
        GameManager.i.gameBoard[x, y] = null;
        Destroy(this.gameObject);
    }

    public override void UpdateHp(int attack)
    {
        CurrentHp -= attack;
        hpText.text = $"{CurrentHp}";
        if (CurrentHp <= 0)
        {
            Die();
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
                transform.position = origin;
            }
			else
			{
                if(hasSummonSickness)
                    disabled.SetActive(true);

                cardText.SetActive(false);
                GameManager.i.availableCardSlots[handIndex] = null;
                GameManager.i.UpdateMana(cost);
                hasBeenPlayed = true;
                x = currentPlacement.x;
                y = currentPlacement.y;
                GameManager.i.gameBoard[x,y] = this;
                transform.position = new Vector3(currentPlacement.transform.position.x, currentPlacement.transform.position.y+.1f, currentPlacement.transform.position.z);
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
    private void OnMouseDown()
    {
        if (isViewMode)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (hasBeenPlayed)
            {
                ViewCard();
            }
            else
            {

                if (!isSelected)
                {
                    if (GameManager.i.CurrentMana >= cost)
                    {
                        origin = transform.position;
                        GameManager.i.selectedCard = this;
                        isSelected = true;
                    }
                }
                //Instantiate(hollowCircle, transform.position, Quaternion.identity);
                // camAnim.SetTrigger("shake");

            }
        }
	}

    private Card ViewCard()
    {
        Card infoCard = Instantiate(this, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 1, Camera.main.transform.position.z + 3), Camera.main.transform.rotation);
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

}
