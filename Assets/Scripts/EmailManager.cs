using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailManager : MonoBehaviour
{
    public static EmailManager emailMan;

    [SerializeField]
    Transform timeLine;

    [SerializeField]
    GameObject emailPrefab;

    [SerializeField]
    List<GameObject> emails = new List<GameObject>();

    [SerializeField]
    EmailContents emailContents;

    private void Start()
    {
        emailMan = this;

        for(int i = 0; i < 20; i++)
        {
            newEmail("Mick Hawk", "Got this really awesome new book for you here. It's about some gay people who really like to eat shit and they fucking love to do it so mycg og my god", 0);
        }
    }

    void newEmail(string author, string subject, int bookIndex = -1)
    {
        AudioManager.audioMan.NewEmail();

        GameObject newEmail = Instantiate(emailPrefab);
        newEmail.transform.SetParent(timeLine);

        emails.Add(newEmail);

        Email emailLogic = newEmail.GetComponent<Email>();
        emailLogic.setUp(author, subject);

        emailLogic.bookIndex = bookIndex;
    }

    public void ClickedOnEmail(string sender, string subject, int bookIndex)
    {
        emailContents.Setup(sender, subject, bookIndex);
        emailContents.Show();
    }
}
