using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int maxHp;
    private int currentHp;
    public TextMeshPro hpText;
    public int x;
    public int y;
    public int affinity = 0;
    public GameObject beingAttacked;
    public int CurrentHp { get => currentHp; set => currentHp = value; }

    // Start is called before the first frame update

    public virtual void UpdateHp(int attack)
    {
    }

    public virtual void Die()
    {
       gameObject.SetActive(false);
    }
}
