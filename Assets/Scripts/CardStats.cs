using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class CardStats
    {
        public int id;
        public int cost;
        public int maxHp;
        public int attack;
        public int speed;
        public int attackPattern;
        public int affinity;
        public string abilityText;
        public List<int> abilities;
        public CardStats(int id, int affinity, int cost, int maxHp, int attack, int speed, int attackPattern = (int)Enums.AttackTypes.First, List<int> abilities = null, string abilityText = "None") {
            this.id = id;
            this.cost = cost;
            this.maxHp = maxHp; 
            this.attack = attack;
            this.speed = speed;
            this.attackPattern = attackPattern;
            this.affinity = affinity;
            if (abilities == null)
            {
                abilities = new List<int>() { (int)Enums.Ability.None };
            }
            this.abilities = abilities;
            this.abilityText = abilityText;
        }
    }
}
