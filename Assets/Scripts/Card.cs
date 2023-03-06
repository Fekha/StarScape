using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Card : Target
{
	public bool hasBeenPlayed;
	public bool isSelected;
    public bool isTeam;
    public bool hasSummonSickness = true;
	public int handIndex;
    public TextMeshPro attackText;
    public TextMeshPro costText;
    public TextMeshPro speedText;
    public GameObject disabled;
    public GameObject inAction;
    public GameObject cardText;
    public Vector3 origin;
    //private Animator anim;
    //private Animator camAnim;

    //public GameObject effect;
    //public GameObject hollowCircle;

    public int cost = 1;
    public int attack = 5;
    public double speed = 7;


    private void Start()
	{
        hpText.text = $"{hp}";
        attackText.text = $"{attack}";
        costText.text = $"{cost}";
        speedText.text = $"{speed}";
        // anim = GetComponent<Animator>();
        // camAnim = Camera.main.GetComponent<Animator>();
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
            Die();
        }
    }

    public override void Die()
    {
        GameManager.i.gameBoard[x, y] = null;
        Destroy(this.gameObject);
    }

    public override void updateColor()
    {
      
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
    private void OnMouseDown()
    {
		if (hasBeenPlayed)
		{
			//show card stats
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
