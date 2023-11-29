using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Button : MonoBehaviour
    {
    // Start is called before the first frame update
    [SerializeField] private Sprite noHovering;
    [SerializeField] private Sprite hovering;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClick() 
    {
        //SFXManager.Instance.PlayAudio("Click");
    }
    public void Hover()
    {
        //SFXManager.Instance.PlayAudio("Hover");
        image.sprite = hovering;
    }

    public void exitHover()
    {
        image.sprite = noHovering;
    }
}