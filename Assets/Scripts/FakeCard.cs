using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class FakeCard : CardStats
    {
        public FakeCard(int id, int affinity, int cost, int maxHp, int attack, int speed, int target, string targetText, List<int> abilities, string abilityText)
        {
            this.id = id;
            this.cost = cost;
            this.maxHp = maxHp;
            this.attack = attack;
            this.speed = speed;
            this.targetId = target;
            this.affinity = affinity;
            this.abilities = abilities;
            this.ability = abilityText;
            this.target = targetText;
        }
    }
}
