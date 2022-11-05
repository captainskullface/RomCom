using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateTime : MonoBehaviour
{
    [SerializeField]
    TMP_Text date;

    [SerializeField]
    TMP_Text time;

    string[] dateTime;

    private void Start()
    {
        dateTime = new string[3];
    }

    private void Update()
    {
        dateTime = System.DateTime.Now.ToString().Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
        date.text = dateTime[0];
        time.text = dateTime[1] + " " + dateTime[2];
    }
}
