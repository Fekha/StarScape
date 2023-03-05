using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int hp = 20;
    public TextMeshPro hpText;
    public int x;
    // Start is called before the first frame update

    public void UpdateHp(int attack)
    {
        hp -= attack;
        hpText.text = $"{hp}";
    }
}
