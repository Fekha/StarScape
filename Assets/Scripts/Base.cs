using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Target
{
    public void Start()
    {
        CurrentHp = maxHp;
        hpText.text = $"{CurrentHp}";
        updateColor();
    }

    public override void UpdateHp(int attack)
    {
        CurrentHp -= attack;
        hpText.text = $"{CurrentHp}";
        updateColor();
        if (CurrentHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void updateColor()
    {
        if (((double)CurrentHp / (double)maxHp) < .3)
            hpText.color = Color.red;
        else if (((double)CurrentHp / (double)maxHp) < .6)
            hpText.color = Color.yellow;
        else
            hpText.color = Color.green;
    }
}
