using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tab : MonoBehaviour
{
    [SerializeField]
    TMP_Text displayText;

    WindowManager manager;

    [SerializeField]
    Color activeColor;

    [SerializeField]
    Color inactiveColor;

    [SerializeField]
    Color hoverColor;

    [SerializeField]
    float expandAmount;

    bool active;
    bool hovering;
    int index;
    RectTransform rect;
    float startingHeight;

    Image image;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        startingHeight = rect.rect.height;
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (active)
            rect.sizeDelta = new Vector2(rect.rect.width, startingHeight + expandAmount);
    }

    public void Setup(string displayName, int indexSet, WindowManager man)
    {
        displayText.text = displayName;
        manager = man;
        index = indexSet;
    }

    private void Update()
    {
        float targetHeight = startingHeight;

        if (hovering || active)
            targetHeight = startingHeight + expandAmount;

        Vector2 targetVector = new Vector2(rect.rect.width, targetHeight);
        rect.sizeDelta = Vector2.MoveTowards(rect.sizeDelta, targetVector, Time.deltaTime * 100);

        Color targetColor = inactiveColor;

        if (active)
            targetColor = activeColor;
        else if (hovering)
            targetColor = hoverColor;

        image.color = Vector4.MoveTowards(image.color, targetColor, Time.deltaTime);
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
        manager.SwitchTo(index);
        active = true;
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
