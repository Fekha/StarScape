using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
	public bool isBeingChosen;
	public int handIndex;

	public Vector3 origin;
	//private Animator anim;
	//private Animator camAnim;

	//public GameObject effect;
	//public GameObject hollowCircle;

	internal double attack =5;
    internal double hp=10;
    internal double speed=7;


    private void Start()
	{
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
                transform.position = (Vector3)currentPlacement;
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

    public void Update()
    {
		if (isBeingChosen)
		{
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
 //   void MoveToDiscardPile()
	//{
	//	Instantiate(effect, transform.position, Quaternion.identity);
	//	gm.discardPile.Add(this);
	//	gameObject.SetActive(false);
	//}
}
