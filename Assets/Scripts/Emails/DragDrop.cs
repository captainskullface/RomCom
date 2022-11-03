using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//this goes on the obj taht is being dragged around

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas; //canvas that ui element will be displayed on

    private RectTransform rect;
    private CanvasGroup itemCanvas;
    Vector3 startScale;
    bool dragged = false;
    public Animator deletedAnim;
    public Animator windowAnims;
   
 
    private void Awake() 
    {
        rect = gameObject.GetComponent<RectTransform>();
        itemCanvas = GetComponent<CanvasGroup>();
        startScale = rect.localScale;
    }

    //--------------------------------------------------------------------
   
    //called when obj is first clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        
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
    }

    private void Update()
    {

        if (dragged)
        {
            Vector3 zoom = new Vector3(Mathf.Clamp(rect.localScale.x * 0.03f, 1.1f, 108), Mathf.Clamp(rect.localScale.y * 0.03f, 1.1f, 108));
            rect.localScale = Vector3.MoveTowards(rect.localScale, zoom, 10f * Time.deltaTime);
        }
        else if(!dragged)
        {
            rect.localScale = Vector3.MoveTowards(rect.localScale, startScale, 10f * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "delete")
        {
            Debug.Log("OMGGGGGG");
            windowAnims.SetBool("isTrashing", true);
        }
    }




}
