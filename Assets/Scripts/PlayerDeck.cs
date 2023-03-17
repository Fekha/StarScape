using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public static PlayerDeck i;
    public List<int> cardsInDeck = new List<int>();
    // Start is called before the first frame update
    void Awake()
    {
        i = this;
        DontDestroyOnLoad(this);
    }

}
