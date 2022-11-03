using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Email : MonoBehaviour
{
    [SerializeField]
    TMP_Text senderDisplay;
    [SerializeField]
    TMP_Text subjectDisplay;
    [SerializeField]
    TMP_Text timeDisplay;

    int timeSincePost = 0;

    Color normalColor;

    [SerializeField]
    Color hoverColor;

    [SerializeField]
    Color readColor;

    [SerializeField]
    float expandAmount;

    bool hovering;

    RectTransform rect;
    Image image;
    float startingHeight;

    [HideInInspector]
    public string contents;

    int bookIndex;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        startingHeight = rect.rect.height;
        image = GetComponent<Image>();
        normalColor = image.color;

        InvokeRepeating("UpdateTime", 0, 60f);
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.rect.width, startingHeight + expandAmount);
    }

    public void setUp(string authorText, string subjectText, string contentSet, int indexSet)
    {
        senderDisplay.text = authorText;
        subjectDisplay.text = subjectText;
        contents = contentSet;
        bookIndex = indexSet;
    }

    private void Update()
    {
        float targetHeight = startingHeight;

        if (hovering)
            targetHeight = startingHeight + expandAmount;

        Vector2 targetVector = new Vector2(rect.rect.width, targetHeight);
        rect.sizeDelta = Vector2.MoveTowards(rect.sizeDelta, targetVector, Time.deltaTime * 100);

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
        Debug.Log(bookIndex);
        EmailManager.emailMan.ClickedOnEmail(senderDisplay.text, subjectDisplay.text, contents, bookIndex);
        normalColor = readColor;
    }

    void UpdateTime()
    {
        string timeText = timeSincePost + "m";

        if (timeSincePost <= 1)
            timeText = "Just Now!";

        float hours = timeSincePost / 60;
        if (hours >= 1)
            timeText = hours.ToString("F0") + "h";

        float days = hours / 24;
        if (days >= 1)
            timeText = days.ToString("F0") + "d";

        float weeks = days / 7;
        if (weeks >= 1)
            timeText = weeks.ToString("F0") + "w";

        timeDisplay.text = timeText;

        timeSincePost++;
    }
}
