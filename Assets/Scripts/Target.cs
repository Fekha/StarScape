using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int maxHp;
    public int hp;
    public TextMeshPro hpText;
    public int x;
    public int y;
    // Start is called before the first frame update
    public void Start()
    {
        hpText.text = $"{hp}";
        updateColor();
    }
    public void UpdateHp(int attack)
    {
        hp -= attack;
        hpText.text = $"{hp}";
        updateColor();
        if (hp <= 0)
        {
            Die();
        }   
    }
    public virtual void updateColor()
    {
        if (((double)hp / (double)maxHp) < .3)
            hpText.color = Color.red;
        else if (((double)hp / (double)maxHp) < .6)
            hpText.color = Color.yellow;
        else
            hpText.color = Color.green;
    }
    public virtual void Die()
    {
       gameObject.SetActive(false);
    }
}
