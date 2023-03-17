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
        public TextMeshProUGUI affinityText;
        public TextMeshProUGUI nameText;
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
            targetText.text = $"{target}";
            nameText.text = $"{name}";
        }

        public virtual void setAffinity()
        {
            switch (affinity)
            {
                case (int)Enums.Affinities.Antimatter:
                    var lightPurple = new Color32(142, 124, 195, 255);
                    affinityColor.color = lightPurple;
                    affinityText.color = lightPurple;
                    affinityText.text = "Antimatter";
                    break;
                case (int)Enums.Affinities.Electrical:
                    var lightBlue = new Color32(109, 158, 235, 255);
                    affinityColor.color = lightBlue;
                    affinityText.color = lightBlue;
                    affinityText.text = "Electrical";
                    break;
                case (int)Enums.Affinities.Thermal:
                    var lightRed= new Color32(224, 102, 102, 255);
                    affinityColor.color = lightRed;
                    affinityText.color = lightRed;
                    affinityText.text = "Thermal";
                    break;
                case (int)Enums.Affinities.Chemical:
                    var lightGreen = new Color32(147, 196, 125, 255);
                    affinityColor.color = lightGreen;
                    affinityText.color = lightGreen;
                    affinityText.text = "Chemical";
                    break;
            }
        }
    }
}
