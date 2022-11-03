using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EmailContents : MonoBehaviour
{
    [SerializeField]
    TMP_Text senderDisplay;
    [SerializeField]
    TMP_Text subjectDisplay;
    [SerializeField]
    TMP_Text contentDisplay;
    [SerializeField]
    DragDrop synopsisHolder;

    [SerializeField]
    GameObject viewBookButton;

    float fadeTime = 0.5f;
    float growTime = 0.375f;
    [SerializeField]
    CanvasGroup contents;
    [SerializeField]
    RectTransform panel;
    float offset = 60;
    Vector2 size;
    Vector2 closedSize;
    CanvasGroup self;

    int bookIndex;

    string bookSynopsis;

    private void Awake()
    {
        self = GetComponent<CanvasGroup>();
        size = new Vector2(Screen.width - (offset * 2), Screen.height - (offset * 2));
        closedSize = new Vector2(size.x, 0);
        panel.sizeDelta = closedSize;

        contents.blocksRaycasts = false;
        contents.interactable = false;
        contents.alpha = 0;

        self.blocksRaycasts = false;
        self.interactable = false;
        self.alpha = 0;
    }

    private void Start()
    {
        //Show();
    }

    public void Setup(string sender, string subject, string contents, int index)
    {
        senderDisplay.text = InkHandler.ProcessText(sender);
        subjectDisplay.text = InkHandler.ProcessText(subject);
        contentDisplay.text = InkHandler.ProcessText(contents);

        bookIndex = index;

        viewBookButton.SetActive(false);

        if (bookIndex >= 0)
        {
            if (!InkHandler.inkMan.books[bookIndex].published && !InkHandler.inkMan.books[bookIndex].rejected)
            {
                bookSynopsis = InkHandler.inkMan.books[index].synopsis;
                viewBookButton.SetActive(true);
            }
        }
    }

    public void Show()
    {
        Tweener open = DOTweenModuleUI.DOSizeDelta(panel, size, growTime);
        TweenCallback callBack = new TweenCallback(ShowContents);
        open.OnComplete(callBack);
        open.Play();

        self.blocksRaycasts = true;
        self.interactable = true;
        Tweener cover = DOTweenModuleUI.DOFade(self, 1, growTime);
        cover.Play();
    }

    public void Hide()
    {
        self.blocksRaycasts = false;
        self.interactable = false;

        contents.blocksRaycasts = false;
        contents.interactable = false;
        Tweener fade = DOTweenModuleUI.DOFade(contents, 0, fadeTime);
        TweenCallback callBack = new TweenCallback(HideSelf);
        fade.OnComplete(callBack);
        fade.Play();
    }

    void ShowContents()
    {
        Tweener fade = DOTweenModuleUI.DOFade(contents, 1, fadeTime);
        fade.Play();
        contents.blocksRaycasts = true;
        contents.interactable = true;
    }

    void HideSelf()
    {
        synopsisHolder.TurnOff();

        Tweener uncover = DOTweenModuleUI.DOFade(self, 0, growTime);
        uncover.Play();

        Tweener close = DOTweenModuleUI.DOSizeDelta(panel, closedSize, growTime);
        close.Play();
    }

    public void PublishHeldBook(int marketedDemo)
    {
        InkHandler.BookStats bookInfo = InkHandler.inkMan.books[bookIndex];
        PublishingManager.publishMan.PublishBook(bookInfo.genre, bookInfo.subGenre, bookInfo.isSequel, bookInfo.targetDemo, marketedDemo, bookInfo.quality, bookInfo.title, bookIndex);
    }

    public void RejectHeldBook()
    {
        InkHandler.inkMan.books[bookIndex].rejected = true;
    }
    public void ShowSynopsis()
    {
        if(!InkHandler.inkMan.books[bookIndex].published)
            synopsisHolder.TurnOn(bookSynopsis);
    }
}
