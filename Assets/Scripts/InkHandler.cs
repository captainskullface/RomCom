using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.Linq;

public class InkHandler : MonoBehaviour
{
    public static InkHandler inkMan;

    public string companyName;

    [SerializeField]
    Vector2 newBookInterval;

    [SerializeField]
    Vector2 followUpEmailDelayRange;

    [SerializeField]
    Vector2 randomEmailInterval;

    [SerializeField]
    Vector2 randomScreechInterval;
    
    [SerializeField, Tooltip("This is a tuning variable so that the rate of everything can be increased with one value")]
    float gameSpeed;

    List<string> randomScreechBank = new List<string>();
    List<string> availableRandomScreech = new List<string>();

    [SerializeField]
    TextAsset tutorialEmail;

    int booksDone = 0;

    [System.Serializable]
    public class BookStats
    {
        public bool published;
        public bool rejected;
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
        public TextAsset associatedScreeches;
        public TextAsset followUpEmail;
        public int sequelIndex;
        public BookStats(string _title, string _author, string _subjectLine, string _emailContents, string _synopsis, int _genre, int _subGenre, int _isSequel, int _targetDemo, int _quality, TextAsset _associatedScreeches, TextAsset _followUpEmail, int _sequelIndex)
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
            associatedScreeches = _associatedScreeches;
            followUpEmail = _followUpEmail;
            sequelIndex = _sequelIndex;
        }
    }

    [SerializeField]
    public List<BookStats> books = new List<BookStats>();

    [SerializeField]
    List<TextAsset> bookData = new List<TextAsset>();

    [SerializeField]
    List<TextAsset> followUpEmails = new List<TextAsset>();

    [SerializeField]
    List<TextAsset> bookScreeches = new List<TextAsset>();

    [SerializeField]
    public List<BookStats> sequels = new List<BookStats>();

    [SerializeField]
    List<TextAsset> sequelData = new List<TextAsset>();

    [SerializeField]
    List<TextAsset> sequelFollowUpEmails = new List<TextAsset>();

    [SerializeField]
    List<TextAsset> sequelScreeches = new List<TextAsset>();

    [SerializeField]
    List<TextAsset> randomEmails = new List<TextAsset>();

    public TextAsset usernameStarters;
    public TextAsset usernameFillers;

    [SerializeField]
    TextAsset randomScreeches;

    //Story story;
    //Story random;

    private void Start()
    {
        //random = new Story(randomScreeches.text);
        inkMan = this;

        ParseSequels();
        ParseBooks();

        companyName = InputName.companyName;

        StartCoroutine(BookFlow());
        StartCoroutine(SpamMailFlow());
        StartCoroutine(RandomScreechFlow());

        Story randScreeches = new Story(randomScreeches.text); ;

        while (randScreeches.canContinue)
        {
            string screech = randScreeches.Continue();
            randomScreechBank.Add(screech);
        }
        availableRandomScreech.AddRange(randomScreechBank);

        Invoke("SendTutorial", 1f);
    }

    void SendTutorial()
    {
        StoryEmail(tutorialEmail);
    }

    IEnumerator BookFlow()
    {
        yield return new WaitForSeconds(Random.Range(newBookInterval.x, newBookInterval.y) * gameSpeed);
        int bookSelect = booksDone;
        booksDone++;
        NewEmail(true, bookSelect);
        if(booksDone < books.Count - 1)
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
            TextAsset screeches = bookScreeches[i];
            TextAsset followUp = followUpEmails[i];

            int sequelIndex = -1;
            //This means theres a sequel
            if(book.canContinue)
            {
                string sequelTitle = book.Continue();
                for (int s = 0; s < sequels.Count; s++)
                {
                    if (sequelTitle.Contains(sequels[s].title))
                        sequelIndex = s;
                }
            }


            synopsis = ProcessText(synopsis);

            subjectLine = ProcessText(subjectLine);

            emailContents = ProcessText(emailContents);

            books.Add(new BookStats(title, author, subjectLine, emailContents, synopsis, genre, subGenre, isSequel, targetDemo, quality, screeches, followUp, sequelIndex));
        }

        books.Shuffle();
        //books.Reverse();
    }

    void ParseSequels()
    {
        for (int i = 0; i < sequelData.Count; i++)
        {
            Story sequelBook = new Story(sequelData[i].text);
            string title = sequelBook.Continue();
            string author = sequelBook.Continue();
            string subjectLine = sequelBook.Continue();
            string emailContents = sequelBook.Continue();
            string synopsis = sequelBook.Continue();
            int genre = int.Parse(sequelBook.Continue());
            int subGenre = int.Parse(sequelBook.Continue());
            int isSequel = int.Parse(sequelBook.Continue());
            int targetDemo = int.Parse(sequelBook.Continue());
            int quality = int.Parse(sequelBook.Continue());
            TextAsset screeches = sequelScreeches[i];
            TextAsset followUp = sequelFollowUpEmails[i];

            synopsis = ProcessText(synopsis);

            subjectLine = ProcessText(subjectLine);

            emailContents = ProcessText(emailContents);

            sequels.Add(new BookStats(title, author, subjectLine, emailContents, synopsis, genre, subGenre, isSequel, targetDemo, quality, screeches, followUp, -1));
        }
    }

    public void NewEmail(bool isBook, int index, bool sequelBook = false)
    {
        if(isBook)
        {
            BookStats pitchedBook = books[index];

            if (sequelBook)
            {
                books.Add(sequels[index]);
                pitchedBook = sequels[index];
                index = books.Count - 1;
            }

            EmailManager.emailMan.newEmail(pitchedBook.author, pitchedBook.subjectLine, pitchedBook.emailContents, index);
        }
        else if(randomEmails.Count > 0)
        {
            int randomPick = Random.Range(0, randomEmails.Count);

            Story email = new Story(randomEmails[randomPick].text);
            string sender = email.Continue();
            string subject = email.Continue();
            string contents = email.Continue();

            EmailManager.emailMan.newEmail(sender, subject, contents);

            randomEmails.RemoveAt(index);
        }
    }

    IEnumerator FollowUpEmail(int index)
    {
        yield return new WaitForSeconds(Random.Range(followUpEmailDelayRange.x, followUpEmailDelayRange.y) * gameSpeed);
        StoryEmail(books[index].followUpEmail);

        if(books[index].sequelIndex >= 0)
        {
            StartCoroutine(SequelEmail(books[index].sequelIndex));
        }
    }

    IEnumerator SequelEmail(int index)
    {
        yield return new WaitForSeconds(Random.Range(followUpEmailDelayRange.x, followUpEmailDelayRange.y) * gameSpeed);
        NewEmail(true, index, true);
    }

    void StoryEmail(TextAsset emailText)
    {
        Story email = new Story(emailText.text);
        string sender = email.Continue();
        string subject = email.Continue();
        string contents = email.Continue();

        EmailManager.emailMan.newEmail(sender, subject, contents);
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

        books[index].published = true;

        Story screeches = new Story(books[index].associatedScreeches.text);
        while(screeches.canContinue)
        {
            string screech = screeches.Continue();
            ScreecherManager.screecherMan.newScreech(screech);
        }

        StartCoroutine(FollowUpEmail(index));

        //books.RemoveAt(index);
        //screechBanks.RemoveAt(index);
    }

    void RandomScreech()
    {
        /*
        if (!randScreeches.canContinue)
            randScreeches.ResetCallstack();
        ScreecherManager.screecherMan.newScreech(randScreeches.Continue() +"/");
        */
        int index = Random.Range(0, availableRandomScreech.Count);
        string rand = availableRandomScreech[index];
        availableRandomScreech.RemoveAt(index);

        if (availableRandomScreech.Count == 0)
            availableRandomScreech.AddRange(randomScreechBank);

        ScreecherManager.screecherMan.newScreech(rand + "/");
    }

    public static string ProcessText(string entry)
    {
        entry = entry.Replace("~", "\n");
        entry = entry.Replace("^", InkHandler.inkMan.companyName);
        entry = entry.Replace("$", "#");
        entry = entry.Replace("&", PublishingManager.publishMan.lastPublished.TrimEnd());
        return entry;
    }
}
