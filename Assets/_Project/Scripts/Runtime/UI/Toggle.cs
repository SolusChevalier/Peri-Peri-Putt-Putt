using UnityEngine;
using UnityEngine.UI;

public class Toggling : MonoBehaviour
{
    private Toggle toggle;
    [SerializeField] private Image toggleBackground;
    [SerializeField] private Image toggleOnImage;
    [SerializeField] private Sprite onDefault;
    [SerializeField] private Sprite onHover;
    [SerializeField] private Sprite offDefault;
    [SerializeField] private Sprite offHover;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        Color c = toggleBackground.color;
        c.a = 0;
        toggleBackground.color = c;
    }
    public void hover()
    {
        if (toggle.isOn)
        {
            toggleOnImage.sprite = onHover;
        }
        else
        {
            toggleBackground.sprite = offHover;
        }
    }
    public void exitHover()
    {
        if (toggle.isOn)
        {
            toggleOnImage.sprite = onDefault;
        }
        else
        {
            toggleBackground.sprite = offDefault;
        }
    }

    public void hideBackground()
    {
        Color c = toggleBackground.color;
        if (toggle.isOn)
        {

            c.a = 0;
            
        }
        else
        {
            c.a = 1;
        }
        toggleBackground.color = c;
    }

}

