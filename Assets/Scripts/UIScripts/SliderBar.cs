using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class SliderBar : MonoBehaviour
    {
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public void SetMaxValue(float value)
        {
            slider.maxValue = value;
            slider.value = value;
            fill.color = gradient.Evaluate(1f);
        }

        public void SetValue(float value)
        {
            slider.value = value;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}