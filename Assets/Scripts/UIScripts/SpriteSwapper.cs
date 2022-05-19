using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float[] thresholds;

    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void SetValue(float value)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (value < thresholds[i])
            {
                if (sprites[i] != null)
                {
                    _image.sprite = sprites[i];
                    _image.material.mainTexture = sprites[i].texture;
                }

                return;
            }
        }
    }
}