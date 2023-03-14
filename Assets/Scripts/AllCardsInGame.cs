using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class AllCardsInGame : MonoBehaviour
    {
        public static AllCardsInGame i;
        void Awake()
        {
            i = this;
            allCards.Add(new FakeCard(0, 3, 1, 4, 4, 4, 0, "First", new List<int>() { }, "None"));
            allCards.Add(new FakeCard(1, 2, 1, 2, 6, 5, 3, "Station", new List<int>() { }, "None"));
            allCards.Add(new FakeCard(2, 1, 1, 10, 4, 5, 2, "Last", new List<int>() { 1 }, "Slow"));
            allCards.Add(new FakeCard(3, 0, 1, 1, 2, 8, 0, "First", new List<int>() { 3 }, "Stealth"));
            allCards.Add(new FakeCard(4, 0, 2, 24, 3, 3, 0, "First", new List<int>() { 1, 2 }, "Slow, Burnout 2"));
            allCards.Add(new FakeCard(5, 1, 2, 15, 6, 5, 2, "Last", new List<int>() { 2 }, "Burnout 2"));
            allCards.Add(new FakeCard(6, 2, 2, 4, 6, 10, 1, "Skip", new List<int>() { }, "None"));
            allCards.Add(new FakeCard(7, 3, 2, 7, 7, 7, 0, "First", new List<int>() { }, "None"));
            allCards.Add(new FakeCard(8, 3, 3, 20, 6, 5, 4, "Consecutive 1", new List<int>() { 5 }, "Scavenger 2"));
            allCards.Add(new FakeCard(9, 2, 3, 18, 12, 6, 1, "Skip", new List<int>() { }, "None"));
            allCards.Add(new FakeCard(10, 1, 3, 11, 11, 11, 0, "First", new List<int>() { }, "None"));
            allCards.Add(new FakeCard(11, 0, 3, 5, 22, 16, 0, "First", new List<int>() { 1, 5 }, "Slow, Scavenger 2"));
            allCards.Add(new FakeCard(12, 0, 4, 40, 2, 2, 0, "First", new List<int>() { 2 }, "Burnout 2"));
            allCards.Add(new FakeCard(13, 1, 4, 25, 4, 14, 5, "Whole Column", new List<int>() { }, "None"));
            allCards.Add(new FakeCard(14, 2, 4, 15, 15, 15, 0, "First", new List<int>() { }, "None"));
            allCards.Add(new FakeCard(15, 3, 4, 9, 16, 17, 1, "Skip", new List<int>() { 4 }, "Drain 10"));
        }

        public List<CardStats> allCards = new List<CardStats>()
        {
           
            
        };
        public List<Sprite> cardSprites;
    }
}
