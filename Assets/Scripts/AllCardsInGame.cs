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
            allCards.Add(new FakeCard(0, 3, 1, 4, 4, 0, "First", "None", "IonScorp"));
            allCards.Add(new FakeCard(1, 2, 1, 2, 3, 3, "Station", "On Attack: Gain 1 Attack", "Shepherd"));
            allCards.Add(new FakeCard(2, 1, 1, 10, 4, 2, "Last", "Slow", "Purifier"));
            allCards.Add(new FakeCard(3, 0, 1, 5, 8, 6, "First in Left Column", "On Attack: Deals 1 damage to itself", "Harvester"));
            allCards.Add(new FakeCard(4, 3, 1, 2, 2, 0, "First", "Stealth", "Carnation"));
            allCards.Add(new FakeCard(5, 2, 1, 4, 6, 3, "Station", "Twice as likely to Misfire", "Crucialis"));
            allCards.Add(new FakeCard(6, 1, 2, 8, 6, 0, "First", "On Arrival: Heal all allies 3 HP", "Shelter"));
            allCards.Add(new FakeCard(7, 0, 2, 24, 5, 8, "First in Center Column", "On Attack: Deals 2 damage to itself", "Faust"));
            allCards.Add(new FakeCard(8, 1, 2, 12, 6, 7, "First in Right Column", "On Attack: Deals 2 damage to all enemies", "Tycho"));
            allCards.Add(new FakeCard(9, 2, 2, 4, 3, 1, "Skip", "On Arrival: Draw a card", "Mangler"));
            allCards.Add(new FakeCard(10, 3, 2, 7, 7, 0, "First", "None", "Torcher"));
            allCards.Add(new FakeCard(11, 3, 3, 20, 6, 4, "Consecutive", "On Attack: Gains 2 attack", "Berserker"));
            allCards.Add(new FakeCard(12, 2, 3, 18, 14, 6, "First in Left Column", "On Attack: Lose 1 attack", "Cynja"));
            allCards.Add(new FakeCard(13, 1, 3, 11, 11, 0, "First", "None", "Kafa"));
            allCards.Add(new FakeCard(14, 0, 3, 5, 22, 0, "First", "Slow", "Rika"));
            allCards.Add(new FakeCard(15, 0, 4, 40, 2, 9, "Whole Row of First", "Twice as likely to Crit", "Kinetik"));
            allCards.Add(new FakeCard(16, 1, 4, 25, 4, 5, "Whole Column", "Reflect 20% of damage taken back to the attacker.", "Yarrow"));
            allCards.Add(new FakeCard(17, 2, 4, 15, 15, 0, "First", "None", "Larkspur"));
            allCards.Add(new FakeCard(18, 3, 4, 9, 16, 1, "Skip", "On Attack: Heals self 2 damage", "Refine"));
            allCards.Add(new FakeCard(19, 1, 5, 19, 19, 0, "First", "None", "Prospect"));
            allCards.Add(new FakeCard(20, 0, 5, 40, 10, 4, "Consecutive", "On Attack: Deals 4 damage to itself", "Redeemer"));
        }

        public List<CardStats> allCards = new List<CardStats>();
        public List<Sprite> cardSprites;
    }
}
