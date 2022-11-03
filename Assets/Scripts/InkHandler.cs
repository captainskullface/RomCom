using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkHandler : MonoBehaviour
{
    public static InkHandler inkMan;

    public string companyName;

    [SerializeField]
    Vector2 newBookInterval;

    [SerializeField]
    Vector2 randomEmailInterval;

    [SerializeField]
    Vector2 randomScreechInterval;
    
    [SerializeField, Tooltip("This is a tuning variable so that the rate of everything can be increased with one value")]
    float gameSpeed;

    [System.Serializable]
    public class BookStats
    {
        public string title;
        public string author;
        public string subjectLine;
        public string emailContents;
        public string synopsis;
        public int genre;
        public int subGenre;
        public int isSequel;
        public int targetDemo;
        public int quality;
        public BookStats(string _title, string _author, string _subjectLine, string _emailContents, string _synopsis, int _genre, int _subGenre, int _isSequel, int _targetDemo, int _quality)
        {
            title = _title;
            author = _author;
            subjectLine = _subjectLine;
            emailContents = _emailContents;
            synopsis = _synopsis;
            genre = _genre;
            subGenre = _subGenre;
            isSequel = _isSequel;
            targetDemo = _targetDemo;
            quality = _quality;
        }

    }

    [SerializeField]
    public List<BookStats> books = new List<BookStats>();

    [SerializeField]
    List<TextAsset> bookData = new List<TextAsset>();

    [SerializeField]
    List<TextAsset> randomEmails = new List<TextAsset>();

    [SerializeField]
    List<TextAsset> screechBanks = new List<TextAsset>();

    public TextAsset usernameStarters;
    public TextAsset usernameFillers;

    [SerializeField]
    TextAsset randomScreeches;
    Story randScreeches;

    //Story story;
    //Story random;

    private void Start()
    {
        //random = new Story(randomScreeches.text);
        inkMan = this;

        ParseBooks();

        //companyName = InputName.

        randScreeches = new Story(randomScreeches.text);

        StartCoroutine(BookFlow());
        StartCoroutine(SpamMailFlow());
        StartCoroutine(RandomScreechFlow());
    }

    IEnumerator BookFlow()
    {
        yield return new WaitForSeconds(Random.Range(newBookInterval.x, newBookInterval.y) * gameSpeed);
        int bookSelect = Random.Range(0, books.Count);
        NewEmail(true, bookSelect);
        StartCoroutine(BookFlow());
    }

    IEnumerator SpamMailFlow()
    {
        yield return new WaitForSeconds(Random.Range(randomEmailInterval.x, randomEmailInterval.y) * gameSpeed);
        int emailSelect = Random.Range(0, randomEmails.Count);
        NewEmail(false, emailSelect);
        StartCoroutine(SpamMailFlow());
    }

    IEnumerator RandomScreechFlow()
    {
        yield return new WaitForSeconds(Random.Range(randomScreechInterval.x, randomScreechInterval.y) * gameSpeed);
        RandomScreech();
        StartCoroutine(RandomScreechFlow());
    }

    void ParseBooks()
    {
        for(int i = 0; i < bookData.Count; i++)
        {
            Story book = new Story(bookData[i].text);
            string title = book.Continue();
            string author = book.Continue();
            string subjectLine = book.Continue();
            string emailContents = book.Continue();
            string synopsis = book.Continue();
            int genre = int.Parse(book.Continue());
            int subGenre = int.Parse(book.Continue());
            int isSequel = int.Parse(book.Continue());
            int targetDemo = int.Parse(book.Continue());
            int quality = int.Parse(book.Continue());

            synopsis = ProcessText(synopsis);

            subjectLine = ProcessText(subjectLine);

            emailContents = ProcessText(subjectLine);

            books.Add(new BookStats(title, author, subjectLine, emailContents, synopsis, genre, subGenre, isSequel, targetDemo, quality));
        }

        books.Reverse();
    }

    public void NewEmail(bool isBook, int index)
    {
        if(isBook)
        {
            BookStats pitchedBook = books[index];
            EmailManager.emailMan.newEmail(pitchedBook.author, pitchedBook.subjectLine, pitchedBook.emailContents, index);
        }
        else
        {
            Story email = new Story(randomEmails[index].text);
            string sender = email.Continue();
            string subject = email.Continue();
            string contents = email.Continue();

            EmailManager.emailMan.newEmail(sender, subject, contents);

            randomEmails.RemoveAt(index);
        }
    }

    public void NewBookPublished(int index)
    {
        /*
        story = new Story(screechBanks[index].text);
        string screechString = story.Continue();
        Debug.Log(screechString);
        string[] screeches = screechString.Split(char.Parse("~"), System.StringSplitOptions.RemoveEmptyEntries);

        for(int i = 0; i < screeches.Length; i++)
        {
            ScreecherManager.screecherMan.newScreech(screeches[i]);
        }
        */

        Story screeches = new Story(screechBanks[index].text);
        while(screeches.canContinue)
        {
            string screech = screeches.Continue();
            ScreecherManager.screecherMan.newScreech(screech);
        }

        books.RemoveAt(index);
        screechBanks.RemoveAt(index);
    }

    void RandomScreech()
    {
        if (!randScreeches.canContinue)
            randScreeches.ResetCallstack();
        ScreecherManager.screecherMan.newScreech(randScreeches.Continue() +"/");
    }

    public static string ProcessText(string entry)
    {
        entry = entry.Replace("~", "\n");
        entry = entry.Replace("^", InkHandler.inkMan.companyName);
        entry = entry.Replace("$", "#");
        entry = entry.Replace("*", PublishingManager.publishMan.lastPublished);
        return entry;
    }
}
