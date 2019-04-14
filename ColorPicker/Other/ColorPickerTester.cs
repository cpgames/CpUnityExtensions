using UnityEngine;

public class ColorPickerTester : MonoBehaviour
{
    #region Fields
    public new Renderer renderer;
    public ColorPicker picker;
    #endregion

    #region Methods
    // Use this for initialization
    private void Start()
    {
        picker.onValueChanged.AddListener(color =>
        {
            renderer.material.color = color;
        });
        renderer.material.color = picker.CurrentColor;
    }

    // Update is called once per frame
    private void Update() { }
    #endregion
}