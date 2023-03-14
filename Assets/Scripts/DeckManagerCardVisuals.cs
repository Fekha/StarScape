using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts
{
    public class DeckManagerCardVisuals : CardStats
    {
        public Image background;
        public Image affinityColor;
        public TextMeshProUGUI targetText;
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI abilityText;
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
                    affinityColor.color = new Color32(184, 61, 186, 100);
                    break;
                case (int)Enums.Affinities.Electrical:
                    affinityColor.color = new Color32(140, 255, 251, 100);
                    break;
                case (int)Enums.Affinities.Thermal:
                    affinityColor.color = new Color32(236, 28, 36, 100);
                    break;
                case (int)Enums.Affinities.Chemical:
                    affinityColor.color = new Color32(196, 255, 14, 100);
                    break;
            }
        }
    }
}
