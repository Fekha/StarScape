using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class FakeCard : CardStats
    {
        public FakeCard(int id, int affinity, int cost, int maxHp, int attack, int target, string targetText, string abilityText, string nameText)
        {
            this.id = id;
            this.cost = cost;
            this.maxHp = maxHp;
            this.attack = attack;
            this.targetId = target;
            this.affinity = affinity;
            this.name = nameText;
            this.ability = abilityText;
            this.target = targetText;
        }
    }
}
