﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class CardStats : MonoBehaviour
    {
        public int maxHp;
        private int currentHp;
        public int id;
        public int cost;
        public int attack;
        public int speed;
        public int targetId;
        public string ability;
        public string target;
        public int affinity = 0;
        public List<int> abilities;
        public int CurrentHp { get => currentHp; set => currentHp = value; }


        internal virtual void UpdateStats(CardStats stat)
        {
            id = stat.id;
            cost = stat.cost;
            maxHp = stat.maxHp;
            attack = stat.attack;
            speed = stat.speed;
            affinity = stat.affinity;
            ability = stat.ability;
            target = stat.target;
        }
    }
}