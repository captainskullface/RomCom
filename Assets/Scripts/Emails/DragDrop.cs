using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//this goes on the obj taht is being dragged around

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas; //canvas that ui element will be displayed on

    private RectTransform rectTransform;
    private CanvasGroup itemCanvas;
    Vector3 startScale;
    bool dragged = false;
    public Animator deletedAnim;
    public Animator windowAnims;
   
 
    private void Awake() 
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        itemCanvas = GetComponent<CanvasGroup>();
        startScale = rectTransform.localScale;
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
        rectTransform.anchoredPosition += (eventData.delta / canvas.scaleFactor); //moves the rectPos along with the .delta (how the mouse has moved since the last frame) / the scale of the canvas

    }


    //called after letting go
    public void OnEndDrag(PointerEventData eventData)
    {
        dragged = false;
          

    }

    private void Update()
    {

        if (dragged)
        {
            Vector3 zoom = new Vector3(Mathf.Clamp(rectTransform.localScale.x * 0.03f, 95, 108), Mathf.Clamp(rectTransform.localScale.y * 0.03f, 95, 108));
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, zoom, 1.2f * Time.deltaTime);
        }



        if (!dragged)
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, startScale, 1.2f * Time.deltaTime);
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
