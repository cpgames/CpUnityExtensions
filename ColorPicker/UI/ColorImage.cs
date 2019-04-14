using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorImage : MonoBehaviour
{
    #region Fields
    public ColorPicker picker;

    private Image image;
    #endregion

    #region Methods
    private void Awake()
    {
        image = GetComponent<Image>();
        picker.onValueChanged.AddListener(ColorChanged);
    }

    private void OnDestroy()
    {
        picker.onValueChanged.RemoveListener(ColorChanged);
    }

    private void ColorChanged(Color newColor)
    {
        image.color = newColor;
    }
    #endregion
}