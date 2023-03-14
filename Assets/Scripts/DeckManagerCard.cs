using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManagerCard : DeckManagerCardVisuals
{
    // Start is called before the first frame update
    public bool isInDeck = false;

    public void changeParent()
    {
        if (isInDeck)
        {
            DeckManager.i.RemoveCardFromDeck(this);
        }
        else
        {
            DeckManager.i.AddCardToDeck(this); 
        }
    }
}
