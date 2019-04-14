using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace cpGames.core
{
    public class CpDropDown : MonoBehaviour
    {
        #region ShowFilter enum
        public enum ShowFilter
        {
            Auto,
            Never,
            Always
        }
        #endregion

        #region Fields
        public const string EMPTY_NAME = "[Empty]";

        private readonly List<DropDownOption> _options = new List<DropDownOption>();

        public Text selectedText;
        public Image selectedIcon;
        public GameObject filter;
        public InputField filterInput;
        public GameObject content;
        public DropDownOption itemTemplate;
        public Transform root;
        public Transform arrow;
        public bool multiselect;
        public ShowFilter showFilter = ShowFilter.Auto;
        public int showFilterOptionsMin = 5;
        [NonSerialized] public bool suspendCallback = true;
        #endregion

        #region Properties
        public IEnumerable<DropDownOption> Options => _options;
        public IEnumerable<string> EnabledOptions { get { return _options.Where(x => x.IsOn).Select(x => x.name); } }
        #endregion

        #region Methods
        public void FilterEdit(string value)
        {
            foreach (var option in Options)
            {
                option.gameObject.SetActive(option.name.ToLower().Contains(value.ToLower()));
            }
        }

        private void Start()
        {
            UpdateSelection();
        }

        public void AddOption(string label, bool value, Action<bool> toggledCallback, object optionData)
        {
            AddOption(label, null, value, toggledCallback, optionData);
        }

        public void AddOption(string label, Sprite icon, bool value, Action<bool> toggledCallback, object optionData)
        {
            var item = root.AddChild(itemTemplate);
            item.Init(label, icon, value, isOn =>
                {
                    if (suspendCallback)
                    {
                        return;
                    }
                    filterInput.text = string.Empty;
                    if (isOn && !multiselect)
                    {
                        foreach (var x in _options.Where(x => x.IsOn && x != item))
                        {
                            x.IsOn = false;
                        }
                    }
                    UpdateSelection();
                    toggledCallback(isOn);
                    if (isOn && content.activeSelf && !multiselect)
                    {
                        ToggleOptions();
                    }
                },
                optionData);
            if (multiselect)
            {
                item.toggle.group = null;
            }
            _options.Add(item);
            UpdateSelection();
        }

        public void ToggleOptions()
        {
            ToggleOptions(!content.activeSelf);
        }

        public void ToggleOptions(bool show)
        {
            content.SetActive(show);
            arrow.rotation = Quaternion.AngleAxis(show ? 180 : 0, Vector3.forward);
            if (show)
            {
                foreach (var option in Options)
                {
                    option.gameObject.SetActive(true);
                }
                switch (showFilter)
                {
                    case ShowFilter.Auto:
                        filter.SetActive(_options.Count > showFilterOptionsMin);
                        break;
                    case ShowFilter.Always:
                        filter.SetActive(true);
                        break;
                }
                if (filter.activeSelf)
                {
                    filterInput.Select();
                }
            }
        }

        public void UpdateSelection()
        {
            var enabledOptions = EnabledOptions.ToList();
            selectedText.text = enabledOptions.Any()
                ? enabledOptions.ToString(", ")
                : EMPTY_NAME;
            if (selectedIcon != null)
            {
                var option = _options
                    .FirstOrDefault(x => x.IsOn && x.icon.sprite != null);
                selectedIcon.sprite = option?.icon.sprite;
                selectedIcon.gameObject.SetActive(option != null);
            }
        }

        public void Clear()
        {
            _options.ForEach(x => Destroy(x.gameObject));
            _options.Clear();
        }
        #endregion
    }
}