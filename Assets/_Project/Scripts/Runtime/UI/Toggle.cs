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
        Screen.fullScreen = true;
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
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
        SFXManager.Instance.PlayAudio("Toggle");
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
        ToggleFullScreen();
    }

    private void ToggleFullScreen()
    {
        //Screen.fullScreen = !Screen.fullScreen;
        if (Screen.fullScreen)
        {
            Screen.SetResolution(1280, 720, false);
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }


    }

}

