using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
	public bool isBeingChosen;
	public int handIndex;

	GameManager gm;

	private Animator anim;
	private Animator camAnim;

	public GameObject effect;
	public GameObject hollowCircle;

	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
		// anim = GetComponent<Animator>();
		// camAnim = Camera.main.GetComponent<Animator>();
	}
	private void OnMouseDown()
	{
		if (!hasBeenPlayed)
		{
            Instantiate(hollowCircle, transform.position, Quaternion.identity);

			// camAnim.SetTrigger("shake");
			// anim.SetTrigger("move");

            //hasBeenPlayed = true;
            isBeingChosen = true;
            gm.availableCardSlots[handIndex] = true;
			// Invoke("MoveToDiscardPile", 2f);

			
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
    void MoveToDiscardPile()
	{
		Instantiate(effect, transform.position, Quaternion.identity);
		gm.discardPile.Add(this);
		gameObject.SetActive(false);
	}



}
