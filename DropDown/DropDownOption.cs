using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace cpGames.core
{
    public class DropDownOption : MonoBehaviour
    {
        #region Fields
        private ColorBlock _originalColors;

        public Toggle toggle;
        public Image icon;
        public Text labelText;
        public object optionData;
        public ColorBlock highlightedColors;
        #endregion

        #region Properties
        public bool IsOn { get => toggle.isOn; set => toggle.isOn = value; }
        #endregion

        #region Methods
        [UsedImplicitly]
        private void Awake()
        {
            _originalColors = toggle.colors;
        }

        public void Init(string label,
            Sprite iconSprite,
            bool value,
            UnityAction<bool> valueChangedCallback,
            object optionData)
        {
            name = StripName(label);
            this.optionData = optionData;
            if (iconSprite != null && icon != null)
            {
                icon.sprite = iconSprite;
                icon.gameObject.SetActive(true);
            }
            else if (icon != null)
            {
                icon.gameObject.SetActive(false);
            }
            labelText.text = label;
            toggle.isOn = value;
            toggle.onValueChanged.AddListener(valueChangedCallback);
            gameObject.SetActive(true);
        }

        private static string StripName(string label)
        {
            while (label.Contains(">"))
            {
                label = label.Remove(0, label.IndexOf('>') + 1);
                label = label.Remove(label.IndexOf('<'));
            }
            return label;
        }

        public void Highlight(bool highlighted)
        {
            toggle.colors = highlighted ? highlightedColors : _originalColors;
        }
        #endregion
    }
}