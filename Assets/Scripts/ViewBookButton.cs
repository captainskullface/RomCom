using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewBookButton : MonoBehaviour
{
    Color normalColor;

    [SerializeField]
    Color hoverColor;


    float expandAmount = 1.1f;
    bool hovering;

    Vector2 startingSize;
    Vector2 expandedSize;
    Image image;

    [SerializeField]
    EmailContents mainEmail;
    
    private void Start()
    {
        startingSize = transform.localScale;
        expandedSize = startingSize * expandAmount;
        image = GetComponent<Image>();
        normalColor = image.color;
    }

    private void Update()
    {
        Vector2 targetSize = startingSize;

        if (hovering)
            targetSize = expandedSize;

        transform.localScale = Vector2.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 100);

        Color targetColor = normalColor;

        if (hovering)
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
        mainEmail.ShowSynopsis();
    }
}
