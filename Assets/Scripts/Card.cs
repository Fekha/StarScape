using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
	public bool isBeingChosen;
	public int handIndex;
    public TextMeshPro hpText; 
    public TextMeshPro attackText;

    public Vector3 origin;
    //private Animator anim;
    //private Animator camAnim;

    //public GameObject effect;
    //public GameObject hollowCircle;

    internal int hp = 10;
    internal int attack = 5;
    internal double speed = 7;


    private void Start()
	{
        hpText.text = $"{hp}";
        attackText.text = $"{attack}";
        // anim = GetComponent<Animator>();
        // camAnim = Camera.main.GetComponent<Animator>();
    }
	private void OnMouseUp()
	{
        if (isBeingChosen)
        {
            isBeingChosen = false;
			var currentPlacement = GameManager.i.getHighlightedPlacement();
            if (currentPlacement == null)
			{
                transform.position = origin;
            }
			else
			{
                hasBeenPlayed = true;
                GameManager.i.availableCardSlots[handIndex] = null;
                handIndex = 0;
                GameManager.i.gameBoard[currentPlacement.x, currentPlacement.y] = this;
                transform.position = new Vector3(currentPlacement.transform.position.x, currentPlacement.transform.position.y+1, currentPlacement.transform.position.z);
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
            if (!isBeingChosen)
            {
                origin = transform.position;
                isBeingChosen = true;
            }
            //Instantiate(hollowCircle, transform.position, Quaternion.identity);
			// camAnim.SetTrigger("shake");
			
		}
	}
    public void UpdateHp(int attack)
    {
        hp -= attack;
        hpText.text = $"{hp}";
    }
    public void Update()
    {
		if (isBeingChosen)
		{
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            var newPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = newPos;
        }
    }
 //   void MoveToDiscardPile()
	//{
	//	Instantiate(effect, transform.position, Quaternion.identity);
	//	gm.discardPile.Add(this);
	//	gameObject.SetActive(false);
	//}
}
