using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;

public class Screech : MonoBehaviour
{
    [SerializeField]
    TMP_Text usernameDisplay;
    [SerializeField]
    TMP_Text content;
    [SerializeField]
    TMP_Text timeDisplay;

    [SerializeField]
    Image image;

    int timeSincePost = 0;

    private void Start()
    {
        InvokeRepeating("UpdateTime", 0, 60f);
    }

    public void setUp(string contentText, Sprite imageSet = null)
    {
        usernameDisplay.text = PickUsername();
        content.text = contentText;

        if (imageSet != null)
            image.sprite = imageSet;
        else
            image.gameObject.SetActive(false);

        AudioManager.audioMan.ScreechPosted();
    }

    string PickUsername()
    {
        Story starters = new Story(InkHandler.inkMan.usernameStarters.text);
        Story fillers = new Story(InkHandler.inkMan.usernameFillers.text);
        string name = starters.Continue().TrimEnd();
        float chance = Random.Range(0, 100);

        float chanceForSeparator = 50;
        string[] separators = new string[3];
        separators[0] = "-";
        separators[1] = "_";
        separators[2] = ".";
        string separator = separators[Random.Range(0, separators.Length)];
        name = name + (chance < chanceForSeparator ? separator : "");

        name = name + fillers.Continue().TrimEnd();

        float chanceForNumbers = 75;
        chance = Random.Range(0, 100);
        int numbers = Random.Range(10, 5000);
        name = name + (chance < chanceForNumbers ? numbers + "" : "");

        //name = InkHandler.ProcessText(name);

        return "@" + name;
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
