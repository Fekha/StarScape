using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target : CardVisuals
{
    public int x;
    public int y;
    public GameObject beingAttacked;

    public virtual void UpdateHp(int attack)
    {
    }
}
