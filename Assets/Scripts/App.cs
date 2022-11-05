using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class App : MonoBehaviour
{
    [SerializeField]
    TMP_Text displayText;

    WindowManager manager;

    //[SerializeField]
    //Color activeColor;

    [SerializeField]
    Color inactiveColor;

    //[SerializeField]
    //Color hoverColor;

    [SerializeField]
    float expandAmount;

    [HideInInspector]
    public bool active;
    bool hovering;
    int index;
    RectTransform rect;
    float startingHeight;
    float startingWidth;

    Image image;

    Color textColor;
    Color hiddenTextColor;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        startingHeight = rect.rect.height;
        startingWidth = rect.rect.width;
        image = GetComponent<Image>();
    }

    private void Start()
    {
        /*
        if (active)
            rect.sizeDelta = new Vector2(rect.rect.width, startingHeight + expandAmount);
        */

        textColor = displayText.color;
        hiddenTextColor = new Color(textColor.r, textColor.g, textColor.b, 0);

        displayText.color = hiddenTextColor;
    }

    public void Setup(string displayName, int indexSet, WindowManager man, Sprite icon)
    {
        displayText.text = displayName;
        manager = man;
        index = indexSet;

        GetComponent<Image>().sprite = icon;
    }

    private void Update()
    {
        float targetHeight = startingHeight;
        float targetWidth = startingWidth;

        if (hovering)
        {
            targetWidth = startingWidth + expandAmount;
            targetHeight = startingHeight + expandAmount;
        }
            

        Vector2 targetVector = new Vector2(targetWidth, targetHeight);
        rect.sizeDelta = Vector2.MoveTowards(rect.sizeDelta, targetVector, Time.deltaTime * 100);

        //Color targetColor = inactiveColor;
        Color targetColor = inactiveColor;

        Color targetTextColor = hiddenTextColor;

        /*
        if (active)
            targetColor = activeColor;
        else if (hovering)
            targetColor = hoverColor;
        */

        if (hovering)
            targetColor = Color.white;

        if (hovering)
            targetTextColor = textColor;

        image.color = Vector4.MoveTowards(image.color, targetColor, Time.deltaTime * 5);

        displayText.color = Vector4.MoveTowards(displayText.color, targetTextColor, Time.deltaTime * 5);
    }

    public void Hover()
    {
        hovering = true;
    }
    public void EndHover()
    {
        hovering = false;
    }

    public void Clicked()
    {
        manager.ClickApp(index);
    }

    public void Activate()
    {
        active = true;
    }
    public void Deactivate()
    {
        active = false;
    }
}
