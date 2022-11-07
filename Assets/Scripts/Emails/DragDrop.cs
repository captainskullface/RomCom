using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

//this goes on the obj taht is being dragged around

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas; //canvas that ui element will be displayed on

    private RectTransform rect;
    private CanvasGroup itemCanvas;
    Vector3 startScale;
    bool dragged = false;
    //public Animator deletedAnim;
    [SerializeField]
    GameObject deleteWindow;
    RectTransform deleteRect;

    [SerializeField]
    GameObject publishWindow;
    RectTransform publishRect;
    //public Animator windowAnims;

    [SerializeField]
    Scrollbar scroll;

    [SerializeField]
    TMP_Text contents;

    float animTime = 0.35f;

    Vector2 windowSize;

    Collider2D col;
    bool open = false;

    private void Awake() 
    {
        rect = gameObject.GetComponent<RectTransform>();

        windowSize = new Vector2(rect.rect.width, rect.rect.height);

        itemCanvas = GetComponent<CanvasGroup>();
        startScale = rect.localScale;

        deleteRect = deleteWindow.GetComponent<RectTransform>();
        publishRect = publishWindow.GetComponent<RectTransform>();

        publishRect.localScale = Vector3.zero;
        deleteRect.localScale = Vector3.zero;

        publishWindow.SetActive(false);
        deleteWindow.SetActive(false);

        rect.localScale = Vector3.zero;

        col = GetComponent<Collider2D>();
    }

    public void TurnOn(string containedText)
    {
        containedText = containedText.TrimEnd();

        open = true;

        rect.position = new Vector3(Screen.width / 2, Screen.height / 2);

        transform.SetAsLastSibling();

        contents.text = containedText;
        //contents.GetComponent<ContentSizeFitter>().enabled = false;
        contents.fontSize += 1;
        Invoke("FitText", 0.05f);

        Tweener show = rect.DOScale(Vector3.one, animTime);
        show.Play();
    }

    void FitText()
    {
        contents.fontSize -= 1;
        contents.GetComponent<ContentSizeFitter>().enabled = true;
        scroll.value = 1;
    }

    public void TurnOff()
    {
        open = false;

        Tweener close = rect.DOScale(0, animTime);
        close.Play();

        if (deleteWindow.activeInHierarchy)
            HideDelete();

        if (publishWindow.activeInHierarchy)
            HidePublish();
    }

    //--------------------------------------------------------------------
   
    //called when obj is first clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }



    //called while beginning the drag
    public void OnBeginDrag(PointerEventData eventData) 
    {
        //good place to put any feedback anims/noises
        dragged = true;
    }



    //called every frame mid drag
    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += (eventData.delta / canvas.scaleFactor); //moves the rectPos along with the .delta (how the mouse has moved since the last frame) / the scale of the canvas
        transform.SetAsLastSibling();
    }


    //called after letting go
    public void OnEndDrag(PointerEventData eventData)
    {
        dragged = false;

        float edge = 50;

        Vector3 adjustedPos = rect.anchoredPosition;
        Vector3 rectSize = rect.rect.size * rect.localScale.x;

        if (rect.anchoredPosition.x > (Screen.width - edge + (rectSize.x / 2)))
        {
            adjustedPos = new Vector3(Screen.width + ((rectSize.x / 2) - (edge - 5)), adjustedPos.y, adjustedPos.z);
        }
        else if (rect.anchoredPosition.x < edge - (rectSize.x / 2))
        {
            adjustedPos = new Vector3((edge + 5) - (rectSize.x / 2), adjustedPos.y, adjustedPos.z);
        }

        if (rect.anchoredPosition.y > 0)
        {
            adjustedPos = new Vector3(adjustedPos.x, 0 - 5, adjustedPos.z);
        }
        else if (rect.anchoredPosition.y < -Screen.height + (edge * 4))
        {
            adjustedPos = new Vector3(adjustedPos.x, -Screen.height + ((edge * 4) + 5), adjustedPos.z);
        }
        rect.anchoredPosition = adjustedPos;

        col.enabled = false;
        col.enabled = true;

    }

    private void Update()
    {
        if(open)
        {
            if (dragged)
            {
                Vector3 zoom = new Vector3(Mathf.Clamp(rect.localScale.x * 0.03f, 1.1f, 108), Mathf.Clamp(rect.localScale.y * 0.03f, 1.1f, 108));
                rect.localScale = Vector3.MoveTowards(rect.localScale, zoom, 10f * Time.deltaTime);
            }
            else if (!dragged)
            {
                rect.localScale = Vector3.MoveTowards(rect.localScale, startScale, 10f * Time.deltaTime);
            }
        }

        rect.sizeDelta = windowSize;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dragged)
            return;

        if (collision.gameObject.tag == "delete")
        {
            Debug.Log("WTFFFFFF");
            if(publishWindow.activeInHierarchy)
                HidePublish();
            ShowDelete();
            //windowAnims.SetBool("isTrashing", true);
        }
        else if (collision.gameObject.tag == "accept")
        {
            Debug.Log("OMGGGGGG");
            if (deleteWindow.activeInHierarchy)
                HideDelete();
            ShowPublish();
        }
    }

    public void ShowDelete()
    {
        deleteWindow.SetActive(true);
        Tweener open = deleteRect.DOScale(Vector3.one, animTime);
        open.Play();
    }
    public void HideDelete()
    {
        Tweener close = deleteRect.DOScale(0, animTime);
        TweenCallback callBack = new TweenCallback(DisableDelete);
        close.OnComplete(callBack);
        close.Play();
    }
    void DisableDelete()
    {
        deleteWindow.SetActive(false);
    }

    public void ShowPublish()
    {
        publishWindow.SetActive(true);
        Tweener open = publishRect.DOScale(Vector3.one, animTime);
        open.Play();
    }
    public void HidePublish()
    {
        Tweener close = publishRect.DOScale(0, animTime);
        TweenCallback callBack = new TweenCallback(DisablePublish);
        close.OnComplete(callBack);
        close.Play();
    }
    void DisablePublish()
    {
        publishWindow.SetActive(false);
    }

}
