using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkHandler : MonoBehaviour
{
    public static InkHandler inkMan;

    public string companyName;

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

    [SerializeField]
    TextAsset randomScreeches;

    //Story story;
    //Story random;

    private void Start()
    {
        //random = new Story(randomScreeches.text);
        inkMan = this;

        ParseBooks();

        NewEmail(true, 0);
        NewBookPublished(0);
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

            synopsis = synopsis.Replace("~", "\n");

            subjectLine = subjectLine.Replace("^", companyName);

            emailContents = emailContents.Replace("~", "\n");
            emailContents = emailContents.Replace("^", companyName);

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
    }

    void RandomScreech()
    {

    }
}
