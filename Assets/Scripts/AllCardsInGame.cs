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
            allCards.Add(new FakeCard(0, 3, 1, 4, 4, 2, 0, "First", "None", "IonScorp"));
            allCards.Add(new FakeCard(1, 2, 1, 2, 6, 3, 3, "Station", "On Attack: Gain 2 speed", "Shepherd"));
            allCards.Add(new FakeCard(2, 1, 1, 10, 4, 3, 2, "Last", "Slow", "Purifier"));
            allCards.Add(new FakeCard(3, 0, 1, 1, 2, 4, 0, "First", "Stealth", "Harvester"));
            allCards.Add(new FakeCard(4, 0, 2, 24, 5, 1, 8, "First in Center Column", "On Attack: Deals 2 damage to itself", "Carnation"));
            allCards.Add(new FakeCard(5, 1, 2, 15, 6, 2, 7, "First in Right Column", "Twice as likely to Misfire", "Crucialis"));
            allCards.Add(new FakeCard(6, 2, 2, 4, 6, 5, 1, "Skip", "On Arrival: Draw a card", "Shelter"));
            allCards.Add(new FakeCard(7, 3, 2, 7, 7, 4, 0, "First", "None", "Faust"));
            allCards.Add(new FakeCard(8, 3, 3, 20, 6, 2, 4, "Consecutive", "On Attack: Gains 2 attack", "Tycho"));
            allCards.Add(new FakeCard(9, 2, 3, 18, 12, 3, 6, "First in Left Column", "On Attack: Lower targets speed by 2", "Mangler"));
            allCards.Add(new FakeCard(10, 1, 3, 11, 11, 6, 0, "First", "None", "Torcher"));
            allCards.Add(new FakeCard(11, 0, 3, 5, 22, 8, 0, "First", "Slow", "Berserker"));
            allCards.Add(new FakeCard(12, 0, 4, 40, 2, 1, 9, "Whole Row of First", "Twice as likely to Crit", "Cynja"));
            allCards.Add(new FakeCard(13, 1, 4, 25, 4, 7, 5, "Whole Column", "Reflect 20% of damage taken back to the attacker.", "Kafa"));
            allCards.Add(new FakeCard(14, 2, 4, 15, 15, 8, 0, "First", "None", "Rika"));
            allCards.Add(new FakeCard(15, 3, 4, 9, 16, 8, 1, "Skip", "On Attack: Heals self 2 damage", "Kinetik"));
        }

        public List<CardStats> allCards = new List<CardStats>()
        {
           
            
        };
        public List<Sprite> cardSprites;
    }
}
