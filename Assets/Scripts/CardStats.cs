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
        public string attackText;
        public List<int> abilities;
        public CardStats(int id, int affinity, int cost, int maxHp, int attack, int speed, int attackPattern, string attackText, List<int> abilities, string abilityText) {
            this.id = id;
            this.cost = cost;
            this.maxHp = maxHp; 
            this.attack = attack;
            this.speed = speed;
            this.attackPattern = attackPattern;
            this.affinity = affinity;
            this.abilities = abilities;
            this.abilityText = abilityText;
            this.attackText = abilityText;
        }
    }
}
