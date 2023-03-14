using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class CardVisuals : CardStats
    {
        public SpriteRenderer background;
        public SpriteRenderer affinityColor;
        public TextMeshPro attackText;
        public TextMeshPro costText;
        public TextMeshPro speedText;
        public TextMeshPro hpText;
        public TextMeshPro targetText;
        public TextMeshPro abilityText;
        public Sprite antimatter;
        public Sprite electric;
        public Sprite thermal;
        public Sprite chemical;
        private void Start()
        {
            setAffinity();
            background.sprite = AllCardsInGame.i.cardSprites[id];
            CurrentHp = maxHp;
            hpText.text = $"{CurrentHp}";
            attackText.text = $"{attack}";
            costText.text = $"{cost}";
            speedText.text = $"{speed}";
            abilityText.text = $"{ability}";
            targetText.text = $"{target}.";
        }

        public virtual void setAffinity()
        {
            switch (affinity)
            {
                case (int)Enums.Affinities.Antimatter:
                    affinityColor.sprite = antimatter;
                    break;
                case (int)Enums.Affinities.Electrical:
                    affinityColor.sprite = electric;
                    break;
                case (int)Enums.Affinities.Thermal:
                    affinityColor.sprite = thermal;
                    break;
                case (int)Enums.Affinities.Chemical:
                    affinityColor.sprite = chemical;
                    break;
            }
        }
    }
}
