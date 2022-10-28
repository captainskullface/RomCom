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
            //newEmail("Mick Hawk", "Got this really awesome new book for you here. It's about some gay people who really like to eat shit and they fucking love to do it so mycg og my god", "SHOCK. HORROR. A BIG PACKAGE. The Lartex have it all and still want more.The alien race only chases pleasure, and their… reproductive evolutionary traits show for it.Every planet they land on ultimately ends up maimed, pillaged, and covered in their ooze.When Rhiannan Mahoney’s home planet is attacked, they come up with a plan for escape as soon as the Planet Attack Alert System starts sounding off.As their custom - built starship leaves the atmosphere, though, the invaders were already on the hunt.A curious Lartex soldier, Lombarno, notices a ping on a location radar.Intrigued by the sign and pulled towards it with an unexplainable sense of allure, he goes out to inspect the scene on his own. Rhiannan and Lombarno’s fates become linked as soon as their ships make contact. The two commence a battle– not only between planets, but between their fiery loins as well.Lust and disgust mix as the two figure out how to make amends with their pasts and each other.Will Lombarno be able to separate from his aggressive ways, or will Rhiannan have to put it to an end themself ?", 0);
        }
    }

    public void newEmail(string author, string subject, string contents, int bookIndex = -1)
    {
        AudioManager.audioMan.NewEmail();

        GameObject newEmail = Instantiate(emailPrefab);
        newEmail.transform.SetParent(timeLine);

        emails.Add(newEmail);

        Email emailLogic = newEmail.GetComponent<Email>();
        emailLogic.setUp(author, subject, contents, bookIndex);
    }

    public void ClickedOnEmail(string sender, string subject, string contents, int bookIndex)
    {
        emailContents.Setup(sender, subject, contents, bookIndex);
        emailContents.Show();
    }
}
