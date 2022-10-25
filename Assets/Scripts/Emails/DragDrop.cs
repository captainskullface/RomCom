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


    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        itemCanvas = GetComponent<CanvasGroup>();
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

        itemCanvas.blocksRaycasts = false; //enables the drag
    }



    //called after letting go
    public void OnEndDrag(PointerEventData eventData)
    {
        itemCanvas.blocksRaycasts = true;
    }



    //called every frame mid drag
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; //moves the rectPos along with the .delta (how the mouse has moved since the last frame) / the scale of the canvas
    }
}
