using System.Collections.Generic;
using System.Linq;
using cpGames.core.RapidMVC;
using UnityEngine.UI;

namespace cpGames.core
{
    public class CpToggleGroup : ToggleGroup
    {
        #region Fields
        private List<Toggle> _toggles;
        #endregion

        #region Properties
        public Signal<bool> OnValueChanged { get; } = new Signal<bool>();
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();

            _toggles = transform.FindAllChildrenRecursively<Toggle>();
            foreach (var toggle in _toggles)
            {
                toggle.onValueChanged.AddListener(isSelected =>
                {
                    if (!isSelected)
                    {
                        return;
                    }
                    var activeToggle = Active();
                    OnValueChanged.Dispatch(activeToggle.name == "ToggleOn");
                });
            }
        }

        public Toggle Active()
        {
            return ActiveToggles().First();
        }

        public void SetOn(bool isOn)
        {
            if (isOn)
            {
                _toggles.First(x => x.name == "ToggleOn").isOn = true;
            }
            else
            {
                _toggles.First(x => x.name == "ToggleOff").isOn = true;
            }
        }
        #endregion
    }
}