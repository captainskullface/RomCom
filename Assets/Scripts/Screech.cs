using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Screech : MonoBehaviour
{
    [SerializeField]
    TMP_Text usernameDisplay;
    [SerializeField]
    TMP_Text content;
    [SerializeField]
    TMP_Text timeDisplay;

    int timeSincePost = 0;

    private void Start()
    {
        InvokeRepeating("UpdateTime", 0, 60f);
    }

    public void setUp(string contentText)
    {
        usernameDisplay.text = PickUsername();
        content.text = contentText;

        AudioManager.audioMan.ScreechPosted();
    }

    string PickUsername()
    {
        return "@" + Random.ColorHSV().ToString();
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
