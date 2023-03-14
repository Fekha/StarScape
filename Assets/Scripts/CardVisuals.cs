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
        public SpriteRenderer handAffinityColor;
        public SpriteRenderer boardAffinityColor;
        public TextMeshPro targetText;
        public TextMeshPro hpText;
        public TextMeshPro attackText;
        public TextMeshPro costText;
        public TextMeshPro speedText;
        public TextMeshPro abilityText;
        public TextMeshPro affinityText;
        public GameObject HandBorder;
        public GameObject BoardBorder;
        public TextMeshPro boardAbilityText;
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
            boardAbilityText.text = $"{ability}";
            targetText.text = $"{target}";
        }

        public virtual void setAffinity()
        {
            switch (affinity)
            {
                case (int)Enums.Affinities.Antimatter:
                    var lightPurple = new Color32(142, 124, 195, 255);
                    affinityColor.color = lightPurple;
                    handAffinityColor.color = lightPurple;
                    boardAffinityColor.color = lightPurple;
                    affinityText.color = lightPurple;
                    affinityText.text = "Antimatter";
                    break;
                case (int)Enums.Affinities.Electrical:
                    var lightBlue = new Color32(109, 158, 235, 255);
                    affinityColor.color = lightBlue;
                    handAffinityColor.color = lightBlue;
                    boardAffinityColor.color = lightBlue;
                    affinityText.color = lightBlue;
                    affinityText.text = "Electrical";
                    break;
                case (int)Enums.Affinities.Thermal:
                    var lightRed= new Color32(224, 102, 102, 255);
                    affinityColor.color = lightRed;
                    handAffinityColor.color = lightRed;
                    boardAffinityColor.color = lightRed;
                    affinityText.color = lightRed;
                    affinityText.text = "Thermal";
                    break;
                case (int)Enums.Affinities.Chemical:
                    var lightGreen = new Color32(147, 196, 125, 255);
                    affinityColor.color = lightGreen;
                    handAffinityColor.color = lightGreen;
                    boardAffinityColor.color = lightGreen;
                    affinityText.color = lightGreen;
                    affinityText.text = "Chemical";
                    break;
            }
        }
    }
}
